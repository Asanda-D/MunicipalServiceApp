using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MunicipalServicesApp.Models;
using MunicipalServicesApp.Services;
using System.IO;
using System.Threading.Tasks;

namespace MunicipalServicesApp.Controllers
{
    public class AnnouncementsController : Controller
    {
        private readonly AnnouncementsService _service;

        public AnnouncementsController(AnnouncementsService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            var announcements = _service.GetAll();
            return View(announcements);
        }

        public IActionResult Add()
        {
            ViewData["Action"] = "Add";
            return View(new Announcement());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Announcement announcement, IFormFile attachment)
        {
            if (!ModelState.IsValid) return View(announcement);

            if (attachment != null)
            {
                var filePath = Path.Combine("wwwroot/uploads", attachment.FileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                await attachment.CopyToAsync(stream);
                announcement.AttachmentPath = "/uploads/" + attachment.FileName;
            }

            _service.Add(announcement);
            TempData["Info"] = "Announcement added successfully";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var announcement = _service.GetById(id);
            if (announcement == null) return NotFound();
            ViewData["Action"] = "Edit";
            return View(announcement);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Announcement announcement, IFormFile attachment)
        {
            if (!ModelState.IsValid) return View(announcement);

            if (attachment != null)
            {
                var filePath = Path.Combine("wwwroot/uploads", attachment.FileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                await attachment.CopyToAsync(stream);
                announcement.AttachmentPath = "/uploads/" + attachment.FileName;
            }

            _service.Update(announcement);
            TempData["Info"] = "Announcement updated successfully";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            TempData["Info"] = "Announcement deleted successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}