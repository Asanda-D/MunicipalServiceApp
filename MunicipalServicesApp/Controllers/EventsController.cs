using Microsoft.AspNetCore.Mvc;
using MunicipalServicesApp.Models;
using MunicipalServicesApp.Services;
using Microsoft.Extensions.Logging; 
using System;

namespace MunicipalServicesApp.Controllers
{
    public class EventsController : Controller
    {
        private readonly EventsService _eventsService;
        private readonly ILogger<EventsController> _logger;
        private readonly IWebHostEnvironment _env;

        // Constructor: injects the EventsService, Logger, and WebHostEnvironment for file handling
        public EventsController(EventsService eventsService, ILogger<EventsController> logger, IWebHostEnvironment env)
        {
            _eventsService = eventsService;
            _logger = logger;
            _env = env;
        }

        // GET: Events/Index
        // Displays all events, including categories, top recommendations, and recent searches
        public IActionResult Index()
        {
            ViewBag.Categories = _eventsService.GetCategories();
            ViewBag.Recommendations = _eventsService.RecommendTop(5);
            ViewBag.RecentSearches = _eventsService.RecentSearches; 
            var all = _eventsService.GetAllEvents();
            return View(all);
        }

        // POST: Events/Search
        // Searches events based on keyword, category, and date range
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Search(string? keyword, string? category, DateTime? from, DateTime? to)
        {
            ViewBag.Categories = _eventsService.GetCategories();
            var results = _eventsService.Search(keyword, category, from, to);
            ViewBag.Recommendations = _eventsService.RecommendTop(5);
            ViewBag.RecentSearches = _eventsService.RecentSearches; 
            return View("Index", results);
        }

        // GET: Events/ListRecommendations (AJAX partial)
        // Returns a partial view containing the top recommended events
        public IActionResult ListRecommendations()
        {
            var recommendations = _eventsService.RecommendTop(5);
            return PartialView("_Recommendations", recommendations);
        }

        // GET: Events/Add
        // Displays the Add Event form, accessible only by admin users
        public IActionResult Add()
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return Unauthorized();

            ViewBag.Categories = _eventsService.GetCategories();
            return View();
        }

        // POST: Events/Add
        // Handles form submission for adding a new event and optional file upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(EventItem model, IFormFile? attachment)
        {
            ViewBag.Categories = _eventsService.GetCategories();
            
            // Restrict access to admin users
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return Unauthorized();

            // Validate model before processing
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _eventsService.GetCategories();
                return View(model);
            }

            // Handle optional file upload (e.g., images or documents)
            if (attachment != null && attachment.Length > 0)
            {
                var uploadsDir = Path.Combine(_env.WebRootPath, "Uploads");
                if (!Directory.Exists(uploadsDir))
                    Directory.CreateDirectory(uploadsDir);

                var fileName = Path.GetFileName(attachment.FileName);
                var filePath = Path.Combine(uploadsDir, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await attachment.CopyToAsync(stream);
                }

                model.AttachmentPath = "/Uploads/" + fileName;
            }

            // Add event to service and save
            _eventsService.AddEvent(model);
            TempData["Success"] = "Event added successfully!";
            return RedirectToAction("Index");
        }

        // GET: Events/Edit
        // Displays the Edit form for an existing event
        public IActionResult Edit(int id)
        {
            var ev = _eventsService.GetEventById(id);
            if (ev == null)
                return NotFound();

            return View(ev);
        }

        // POST: Events/Edit
        // Updates existing event details and handles optional new file upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EventItem model, IFormFile? attachment)
        {
            if (!ModelState.IsValid)
                return View(model);

            // If a new file is uploaded, save it and update the path
            if (attachment != null && attachment.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(attachment.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    attachment.CopyTo(stream);
                }

                model.AttachmentPath = "/uploads/" + fileName;
            }

            _eventsService.UpdateEvent(model);
            TempData["Info"] = "Event updated successfully!";
            return RedirectToAction("Index", "Events");
        }

        // POST: Events/Delete
        // Deletes an event and redirects back to the Index page
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _eventsService.DeleteEvent(id);
            TempData["Info"] = "Event deleted successfully!";
            return RedirectToAction("Index", "Events");
        }

        // POST: Events/ClearSearches
        // Clears all recent search history and reloads the Index view
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ClearSearches()
        {
            _eventsService.ClearAllSearches();
            TempData["Info"] = "All search history cleared.";
            return RedirectToAction("Index");
        }
    }
}