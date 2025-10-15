using Microsoft.AspNetCore.Mvc;

namespace MunicipalServicesApp.Controllers
{
    public class MainMenuController : Controller
    {
        // GET: MainMenu
        public ActionResult Index()
        {
            // For now, no data is needed; just render the Main Menu
            return View();
        }
    }
}
