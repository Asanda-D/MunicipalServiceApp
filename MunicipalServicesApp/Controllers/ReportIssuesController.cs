using Microsoft.AspNetCore.Mvc;
using MunicipalServicesApp.Models;

namespace MunicipalServicesApp.Controllers
{
    public class ReportIssuesController : Controller
    {
        private readonly IWebHostEnvironment _env;

        public ReportIssuesController(IWebHostEnvironment env)
        {
            _env = env;
        }

        // Temporary in-memory storage
        private static List<Issue> issues = new List<Issue>();

        // Categories
        private static List<string> categories = new List<string>
        {
            "Sanitation",       // Garbage collection, waste management
            "Roads",            // Potholes, street repairs
            "Utilities",        // Water, electricity, sewer issues
            "Public Safety",    // Street lighting, vandalism, crime reporting
            "Parks & Recreation", // Playgrounds, sports fields, green spaces
            "Traffic & Transport", // Signage, traffic lights, public transport
            "Building & Zoning",   // Construction issues, permits
            "Animal Control",      // Stray animals, pet issues
            "Environmental Concerns", // Pollution, water quality, hazards
            "Community Services",  // Libraries, community centers, social services
            "Other"               // Catch-all for miscellaneous reports
        };

        // Motivational messages
        private static string[] motivationalMessages = new string[]
        {
            "Your voice matters! Report issues to improve your community.",
            "Together we can make a difference!",
            "Every report helps the municipality serve you better.",
            "Stay engaged! Your feedback counts.",
            "Thank you for taking action in your community!"
        };

        // GET: ReportIssues/Create
        public IActionResult Create()
        {
            ViewBag.Categories = categories;

            // Pick a random motivational message
            var random = new Random();
            ViewBag.Motivation = motivationalMessages[random.Next(motivationalMessages.Length)];

            return View();
        }

        // POST: ReportIssues/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Issue model, IFormFile attachment)
        {
            Console.WriteLine("POST method hit!");

            // Reload categories for dropdown
            ViewBag.Categories = categories;

            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState invalid!");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Validation Error: {error.ErrorMessage}");
                }

                // Pick a motivational message even on validation fail
                var randomFail = new Random();
                ViewBag.Motivation = motivationalMessages[randomFail.Next(motivationalMessages.Length)];

                return View(model);
            }

            Console.WriteLine($"Valid model. Location={model.Location}, Category={model.Category}");

            // Assign unique Id
            model.Id = issues.Count > 0 ? issues.Max(i => i.Id) + 1 : 1;

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

            // Set timestamp and save
            model.SubmittedAt = DateTime.Now;
            issues.Add(model);

            Console.WriteLine($"Issue saved! Total issues now: {issues.Count}");

            // Pass a success message for inline alert
            TempData["SuccessMessage"] = "Issue submitted successfully! Thank you for reporting.";

            // Redirect to Create page so user sees cleared form + success message
            return RedirectToAction("Create");
        }

        // GET: ReportIssues/List
        public IActionResult List()
        {
            Console.WriteLine($"Returning {issues.Count} issues to List view.");
            return View(issues);
        }
    }
}

