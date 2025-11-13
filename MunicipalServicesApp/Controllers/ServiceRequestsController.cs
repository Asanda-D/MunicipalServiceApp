using Microsoft.AspNetCore.Mvc;
using MunicipalServicesApp.Models;
using MunicipalServicesApp.Services;

namespace MunicipalServicesApp.Controllers
{
    public class ServiceRequestsController : Controller
    {
        private readonly RequestsService _requestsService;

        // Predefined list of service categories
        private static List<string> categories = new List<string>
        {
            "Sanitation",       
            "Roads",            
            "Utilities",        
            "Public Safety",    
            "Parks & Recreation", 
            "Traffic & Transport", 
            "Building & Zoning",   
            "Animal Control",      
            "Environmental Concerns", 
            "Community Services",  
            "Other"               
        };

        // Constructor to initialize the service dependency
        public ServiceRequestsController(RequestsService requestsService)
        {
            _requestsService = requestsService;
        }

        // Display a list of all service requests
        public IActionResult Index()
        {
            var requests = _requestsService.GetAll();
            return View(requests);
        }

        // Display details for a specific request
        public IActionResult Details(int id)
        {
            var request = _requestsService.GetById(id);
            if (request == null)
                return NotFound();

            return View(request);
        }

        // Show the form to create a new request
        public IActionResult Create()
        {
            ViewBag.Categories = categories;
            return View();
        }

        // Handle form submission for new requests
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ServiceRequest model, IFormFile? attachment)
        {
            ViewBag.Categories = categories;

            if (ModelState.IsValid)
            {
                // Handle file upload if present
                if (attachment != null && attachment.Length > 0)
                {
                    var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    if (!Directory.Exists(uploadsPath))
                        Directory.CreateDirectory(uploadsPath);

                    var fileName = Path.GetFileName(attachment.FileName);
                    var filePath = Path.Combine(uploadsPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        attachment.CopyTo(stream);
                    }

                    model.AttachmentPath = "/uploads/" + fileName;
                }

                // Add the new request to the list
                _requestsService.AddRequest(model);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // Show the edit form for an existing request (admin only)
        public IActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return RedirectToAction("Index");

            var request = _requestsService.GetById(id);
            if (request == null)
                return NotFound();

            // Provide all possible status options
            ViewBag.StatusOptions = Enum.GetValues(typeof(RequestStatus));

            return View(request);
        }

        // Handle edit form submission (admin only)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, ServiceRequest model, IFormFile? attachment)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return RedirectToAction("Index");

            var existing = _requestsService.GetById(id);
            if (existing == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                // Handle file update if new one is uploaded
                if (attachment != null && attachment.Length > 0)
                {
                    var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    if (!Directory.Exists(uploadsPath))
                        Directory.CreateDirectory(uploadsPath);

                    var fileName = Path.GetFileName(attachment.FileName);
                    var filePath = Path.Combine(uploadsPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        attachment.CopyTo(stream);
                    }

                    existing.AttachmentPath = "/uploads/" + fileName;
                }

                // Update request details
                existing.Title = model.Title;
                existing.Location = model.Location;
                existing.Category = model.Category;
                existing.Description = model.Description;
                existing.Status = model.Status;
                existing.Priority = model.Priority;

                // Log a status update if the status changed
                if (existing.Status != model.Status)
                {
                    existing.Status = model.Status;
                    existing.Updates.Add(new ServiceRequestUpdate
                    {
                        Status = model.Status,
                        Message = $"Status changed to {model.Status}"
                    });
                }

                _requestsService.UpdateRequest(existing);
                return RedirectToAction("Index");
            }

            ViewBag.StatusOptions = Enum.GetValues(typeof(RequestStatus));

            return View(model);
        }

        // Show delete confirmation page (admin only)
        public IActionResult Delete(int id)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return RedirectToAction("Index");

            var request = _requestsService.GetById(id);
            if (request == null)
                return NotFound();

            return View(request);
        }

        // Handle confirmed deletion (admin only)
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return RedirectToAction("Index");

            _requestsService.Delete(id);
            return RedirectToAction("Index");
        }

        // Display the tracking form
        [HttpGet]
        public IActionResult Track()
        {
            return View(); // Initially show the tracking view
        }

        // Handle request tracking based on user input
        [HttpPost]
        public IActionResult TrackRequest(string srId)
        {
            if (string.IsNullOrEmpty(srId))
                return Json(new { success = false, message = "Please enter a Request ID." });

            if (!srId.StartsWith("SR") || !int.TryParse(srId.Substring(2), out var id))
                return Json(new { success = false, message = "Invalid Request ID." });

            var request = _requestsService.GetById(id);
            if (request == null)
                return Json(new { success = false, message = "No service request found with that ID." });

            // Return the request details in JSON format
            return Json(new
            {
                success = true,
                data = new
                {
                    srId = "SR" + request.Id,
                    request.Title,
                    request.Description,
                    status = request.Status.ToString(),
                    request.Priority,
                    submittedAt = request.SubmittedAt.ToString("g"),
                    attachmentPath = request.AttachmentPath
                }
            });
        }
    }
}