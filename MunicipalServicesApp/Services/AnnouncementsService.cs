using MunicipalServicesApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace MunicipalServicesApp.Services
{
    public class AnnouncementsService
    {
        private readonly List<Announcement> _announcements = new();
        private int _nextId = 1;

        // Get all announcements
        public IEnumerable<Announcement> GetAll() => _announcements.OrderByDescending(a => a.CreatedAt);

        // Get by ID
        public Announcement GetById(int id) => _announcements.FirstOrDefault(a => a.Id == id);

        // Add new announcement
        public void Add(Announcement announcement)
        {
            announcement.Id = _nextId++;
            announcement.CreatedAt = System.DateTime.Now;
            _announcements.Add(announcement);
        }

        // Update existing announcement
        public void Update(Announcement announcement)
        {
            var existing = GetById(announcement.Id);
            if (existing != null)
            {
                existing.Title = announcement.Title;
                existing.Message = announcement.Message;
                existing.AttachmentPath = announcement.AttachmentPath;
            }
        }

        // Delete announcement
        public void Delete(int id)
        {
            var announcement = GetById(id);
            if (announcement != null)
                _announcements.Remove(announcement);
        }

        public void Seed()
        {
            Add(new Announcement
            {
                Title = "Water Maintenance",
                Message = "Water supply will be interrupted on 15 November from 8 AM to 2 PM.",
                AttachmentPath = null
            });

            Add(new Announcement
            {
                Title = "Community Cleanup",
                Message = "Join us this Saturday for a neighborhood cleanup event.",
                AttachmentPath = null
            });

            Add(new Announcement
            {
                Title = "Road Repairs",
                Message = "Main Street will be closed for repairs from 18 to 22 November.",
                AttachmentPath = null
            });
        }
    }
}