using Microsoft.AspNetCore.Mvc;

namespace RoxyBusinessPortolioApp.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
