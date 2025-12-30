using Foras_Khadra.Data;
using Foras_Khadra.Migrations;
using Foras_Khadra.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Foras_Khadra.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context; // إضافة DbContext

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
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

        public IActionResult StatusCode(int code)
        {
            var model = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            return View("Error", model);
        }

        public IActionResult AboutUs()
        {
            var members = _context.TeamMember
                                  .OrderByDescending(m => m.Membership)
                                  .ToList();
            return View(members); // ستبحث عن Views/Home/AboutUs.cshtml
        }

        public IActionResult Articals()
        {
            var articles = _context.Articles
            .OrderByDescending(a => a.PublishDate)
            .ToList();

            var model = new HomeArticlesViewModel
            {
                LatestArticles = articles.Take(5).ToList(),
                OtherArticles = articles.Skip(5).ToList()
            };

            return View(model);
        }
    }
}
