using Microsoft.AspNetCore.Mvc;

namespace MunicipalServicesApp.Controllers
{
    public class MainMenuController : Controller
    {
        // GET: MainMenu
        public ActionResult Index()
        {
            // No data is needed; just rendering the Main Menu
            return View();
        }
    }
}
