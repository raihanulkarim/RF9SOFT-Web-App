using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RoxyBusinessPortolioApp.Data;
using RoxyBusinessPortolioApp.Models;
using System.Diagnostics;
using System.Threading.Tasks;

namespace RoxyBusinessPortolioApp.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context; // Corrected assignment
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new HomeViewModel
            {
                Projects = await _context.ProjectModel.OrderByDescending(r => r.Id).Take(5).ToListAsync()
            };
            return View(viewModel);
        }

        public async Task<IActionResult> Products()
        {
            if (_context.ProjectModel != null)
            {
                var projects = await _context.ProjectModel.OrderByDescending(r => r.Id).ToListAsync();
                return View(projects);
            }
            else
            {
                return Problem("Entity set 'ApplicationDbContext.ProjectModel' is null.");
            }
        }

        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
