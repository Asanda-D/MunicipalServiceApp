using System;
using System.Collections.Generic;
using System.Linq;
using MunicipalServicesApp.Models;

namespace MunicipalServicesApp.Services
{
    // Simple in-memory repository / service demonstrating data structures
    public class EventsService
    {
        private static readonly List<string> categories = new List<string>
        {
            "Community Engagement",
            "Health & Wellness",
            "Education & Training",
            "Environmental Awareness",
            "Public Safety",
            "Infrastructure",
            "Cultural Events",
            "Youth Development",
            "Sports & Recreation",
            "Other"
        };

        // Master list
        private readonly List<EventItem> _events = new();

        // Hash table / dictionary: category -> events
        private readonly Dictionary<string, List<EventItem>> _byCategory = new(StringComparer.OrdinalIgnoreCase);

        // SortedDictionary: date -> list of events that start that day (efficient range queries)
        private readonly SortedDictionary<DateTime, List<EventItem>> _byDate = new();

        // Set of categories
        private readonly HashSet<string> _categories = new(categories, StringComparer.OrdinalIgnoreCase);

        // Recent searches queue (keeps order, FIFO)
        private readonly Queue<string> _recentSearches = new();

        // Search undo stack (LIFO)
        private readonly Stack<string> _searchStack = new();

        // For recommendation scoring we'll use a dictionary to track category frequencies
        private readonly Dictionary<string, int> _searchCategoryCounts = new(StringComparer.OrdinalIgnoreCase);

        // Simple constructor to seed sample data
        public EventsService()
        {
            SeedSampleEvents();
        }

        // Add event and place into data structures
        public void AddEvent(EventItem ev)
        {
            // assign id if not assigned
            if (ev.Id == 0)
            {
                ev.Id = _events.Count > 0 ? _events.Max(e => e.Id) + 1 : 1;
            }

            _events.Add(ev);

            // byCategory
            if (!_byCategory.TryGetValue(ev.Category ?? "Uncategorized", out var list))
            {
                list = new List<EventItem>();
                _byCategory[ev.Category] = list;
            }
            list.Add(ev);

            // byDate - use StartDate.Date as key
            var key = ev.StartDate.Date;
            if (!_byDate.TryGetValue(key, out var dateList))
            {
                dateList = new List<EventItem>();
                _byDate[key] = dateList;
            }
            dateList.Add(ev);

            // categories set
            if (!string.IsNullOrWhiteSpace(ev.Category))
                _categories.Add(ev.Category);
        }

        public IEnumerable<EventItem> GetAllEvents() => _events.OrderBy(e => e.StartDate);

        public IEnumerable<string> GetCategories() => _categories.OrderBy(c => c);

        // Search by keyword, category, and date range (basic)
        public IEnumerable<EventItem> Search(string? keyword, string? category, DateTime? from, DateTime? to)
        {
            // Use dictionaries/sets for faster initial narrowing
            IEnumerable<EventItem> candidates = _events;

            if (!string.IsNullOrWhiteSpace(category))
            {
                if (_byCategory.TryGetValue(category!, out var catList))
                    candidates = catList;
                else
                    return Enumerable.Empty<EventItem>();
            }

            if (from.HasValue || to.HasValue)
            {
                var start = from?.Date ?? DateTime.MinValue.Date;
                var end = to?.Date ?? DateTime.MaxValue.Date;

                // Use SortedDictionary for efficient enumeration over keys in range
                var datesInRange = _byDate.Keys.Where(d => d >= start && d <= end);
                var eventsOnDates = datesInRange.SelectMany(d => _byDate[d]);
                candidates = category == null ? eventsOnDates : candidates.Intersect(eventsOnDates);
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var kw = keyword!.Trim();
                candidates = candidates.Where(e =>
                    e.Title.Contains(kw, StringComparison.OrdinalIgnoreCase) ||
                    e.Description.Contains(kw, StringComparison.OrdinalIgnoreCase) ||
                    e.Location.Contains(kw, StringComparison.OrdinalIgnoreCase)
                );
            }

            // Convert to ordered list
            var result = candidates.OrderBy(e => e.StartDate).ToList();

            // Record the query for recommendations
            RecordSearch(keyword, category);

            return result;
        }

        // Record user queries in queue & stack and update category frequency counts
        private void RecordSearch(string? keyword, string? category)
        {
            string token = $"{category ?? "ALL"}::{keyword ?? ""}";
            _recentSearches.Enqueue(token);
            _searchStack.Push(token);

            // keep queue size limited (e.g., 20)
            while (_recentSearches.Count > 20)
                _recentSearches.Dequeue();

            // Update category counts
            if (!string.IsNullOrWhiteSpace(category))
            {
                _searchCategoryCounts.TryGetValue(category!, out var c);
                _searchCategoryCounts[category!] = c + 1;
            }
        }

        // Recommendation algorithm: based on search history category frequencies + event recency
        // Use PriorityQueue to rank events by a computed score (higher score => higher priority)
        public IEnumerable<EventItem> RecommendTop(int maxRecommendations = 5)
        {
            // If no searches, recommend nearest upcoming events
            if (_searchCategoryCounts.Count == 0)
            {
                return _events
                    .Where(e => e.StartDate >= DateTime.Now)
                    .OrderBy(e => e.StartDate)
                    .Take(maxRecommendations);
            }

            // Build a priority queue of candidate events with score = categoryScore * 1000 - daysUntilStart
            var pq = new PriorityQueue<EventItem, int>();

            foreach (var ev in _events)
            {
                int categoryScore = 0;
                if (!string.IsNullOrWhiteSpace(ev.Category) && _searchCategoryCounts.TryGetValue(ev.Category, out var cs))
                    categoryScore = cs;

                // prefer future events
                int daysUntil = (int)Math.Max(0, (ev.StartDate.Date - DateTime.Now.Date).TotalDays);

                // Higher score => more relevant; PriorityQueue in .NET sorts by smallest priority first,
                // so we invert the score (use negative)
                int score = -(categoryScore * 1000 - daysUntil);
                pq.Enqueue(ev, score);
            }

            var list = new List<EventItem>();
            while (pq.Count > 0 && list.Count < maxRecommendations)
            {
                list.Add(pq.Dequeue());
            }

            return list;
        }

        // Expose some data structure states for demonstration (e.g., UI can show counts)
        public int RecentSearchCount => _recentSearches.Count;
        public IEnumerable<string> RecentSearches => _recentSearches.ToArray();
        public IEnumerable<string> SearchStackSnapshot => _searchStack.ToArray();
        public IDictionary<string, int> SearchCategoryCounts => new Dictionary<string, int>(_searchCategoryCounts);

        // Pops the most recent search from the stack (for Undo feature)
        public string? PopLastSearch()
        {
            if (_searchStack.Count > 0)
            {
                return _searchStack.Pop();
            }
            return null;
        }

        // Get by ID
        public EventItem? GetEventById(int id)
        {
            return _events.FirstOrDefault(e => e.Id == id);
        }

        // Update
        public void UpdateEvent(EventItem updated)
        {
            var existing = _events.FirstOrDefault(e => e.Id == updated.Id);
            if (existing != null)
            {
                existing.Title = updated.Title;
                existing.Description = updated.Description;
                existing.Category = updated.Category;
                existing.StartDate = updated.StartDate;
                existing.EndDate = updated.EndDate;
                existing.Location = updated.Location;
                existing.AttachmentPath = updated.AttachmentPath;
            }
        }

        // Delete
        public void DeleteEvent(int id)
        {
            var existing = _events.FirstOrDefault(e => e.Id == id);
            if (existing != null)
                _events.Remove(existing);
        }

        // Clears all searches from stack and recent searches
        public void ClearAllSearches()
        {
            _searchStack.Clear();
            _recentSearches.Clear();
            _searchCategoryCounts.Clear();
        }

        // Helper to seed sample events
        private void SeedSampleEvents()
        {
            // (Add several events with variety of categories and dates)
            AddEvent(new EventItem
            {
                Title = "Community Cleanup Day",
                Description = "Join neighbours to pick up litter in the park.",
                Category = "Sanitation",
                StartDate = DateTime.Now.AddDays(3),
                EndDate = DateTime.Now.AddDays(3).AddHours(3),
                Location = "Central Park",
                AttachmentPath = null
            });

            AddEvent(new EventItem
            {
                Title = "Road Works: Main Street",
                Description = "Temporary road closure for resurfacing.",
                Category = "Roads",
                StartDate = DateTime.Now.AddDays(7),
                EndDate = DateTime.Now.AddDays(8),
                Location = "Main Street",
                AttachmentPath = null
            });

            AddEvent(new EventItem
            {
                Title = "Water Maintenance",
                Description = "Planned maintenance will affect supply between 09:00 - 16:00",
                Category = "Utilities",
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(1).AddHours(7),
                Location = "Multiple Suburbs",
                AttachmentPath = null
            });

            AddEvent(new EventItem
            {
                Title = "Library Outreach: Youth Reading",
                Description = "Free reading activities for children.",
                Category = "Community Services",
                StartDate = DateTime.Now.AddDays(10),
                EndDate = DateTime.Now.AddDays(10).AddHours(2),
                Location = "Town Library",
                AttachmentPath = null
            });

            AddEvent(new EventItem
            {
                Title = "Pothole Repairs - West End",
                Description = "Pothole repairs on Baker Ave",
                Category = "Roads",
                StartDate = DateTime.Now.AddDays(2),
                EndDate = DateTime.Now.AddDays(2).AddHours(4),
                Location = "Baker Ave",
                AttachmentPath = null
            });
        }
    }
}