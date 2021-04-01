using AdventureWorksERM.Models;
using AdventureWorksERM.Models.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace AdventureWorksERM.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private AdventureWorksContext Context { get; }

        public HomeController(ILogger<HomeController> logger, AdventureWorksContext context)
        {
            this.Context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Index","Product", new { category = 1 });
        }

        public IActionResult Privacy()
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
