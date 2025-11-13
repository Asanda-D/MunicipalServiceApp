using Microsoft.AspNetCore.Mvc;

namespace MunicipalServicesApp.Controllers
{
    public class AdminController : Controller
    {
        // Hardcoded credentials
        private const string AdminUsername = "admin";
        private const string AdminPassword = "Pass123"; 

        // GET: Admin/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Admin/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string username, string password)
        {
            if (username == AdminUsername && password == AdminPassword)
            {
                // Mark admin as logged in
                HttpContext.Session.SetString("IsAdmin", "true");
                return RedirectToAction("Index", "Events"); // go to manage events
            }

            ViewBag.Error = "Invalid username or password";
            return View();
        }

        // GET: Admin/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("IsAdmin");
            return RedirectToAction("Index", "Events");
        }
    }
}
