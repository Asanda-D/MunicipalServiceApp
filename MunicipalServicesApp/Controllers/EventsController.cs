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

        public EventsController(EventsService eventsService, ILogger<EventsController> logger, IWebHostEnvironment env)
        {
            _eventsService = eventsService;
            _logger = logger;
        }

        // GET: Events/Index
        public IActionResult Index()
        {
            ViewBag.Categories = _eventsService.GetCategories();
            ViewBag.Recommendations = _eventsService.RecommendTop(5);
            ViewBag.RecentSearches = _eventsService.RecentSearches; // ← Add this line
            var all = _eventsService.GetAllEvents();
            return View(all);
        }
        // POST: Events/Search
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Search(string? keyword, string? category, DateTime? from, DateTime? to)
        {
            ViewBag.Categories = _eventsService.GetCategories();
            var results = _eventsService.Search(keyword, category, from, to);
            ViewBag.Recommendations = _eventsService.RecommendTop(5);
            ViewBag.RecentSearches = _eventsService.RecentSearches; // ← Add here too
            return View("Index", results);
        }

        // GET: Events/ListRecommendations (AJAX partial)
        public IActionResult ListRecommendations()
        {
            var recommendations = _eventsService.RecommendTop(5);
            return PartialView("_Recommendations", recommendations);
        }

        // GET: Events/Add
        public IActionResult Add()
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return Unauthorized();

            ViewBag.Categories = _eventsService.GetCategories();
            return View();
        }

        // POST: Events/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(EventItem model, IFormFile? attachment)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return Unauthorized();

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _eventsService.GetCategories();
                return View(model);
            }

            // Handle optional file upload
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

            _eventsService.AddEvent(model);
            TempData["Success"] = "Event added successfully!";
            return RedirectToAction("Index");
        }

        // GET: Events/Edit
        public IActionResult Edit(int id)
        {
            var ev = _eventsService.GetEventById(id);
            if (ev == null)
                return NotFound();

            return View(ev);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EventItem model, IFormFile? attachment)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Handle attachment upload
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _eventsService.DeleteEvent(id);
            TempData["Info"] = "Event deleted successfully!";
            return RedirectToAction("Index", "Events");
        }

        // Clear all searches
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