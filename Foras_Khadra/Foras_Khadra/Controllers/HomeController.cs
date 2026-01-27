using Foras_Khadra.Data;
using Foras_Khadra.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var latestArticles = _context.Articles
                .OrderByDescending(a => a.PublishDate)
                .Take(9)
                .AsNoTracking()
                .ToList();

            var model = new HomeViewModel
            {
                LatestArticles = latestArticles
            };

            return View(model);
        }

        public IActionResult News()
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

        public IActionResult Articles()
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

        // ================= الفرص حسب النوع مع فلترة الدولة =================

        // ================= الفرص حسب النوع مع فلترة الدولة =================
        private IActionResult GetOpportunitiesByType(Foras_Khadra.Models.OpportunityType type, string country, string typeName)
        {
            // جلب كل الفرص من قاعدة البيانات حسب النوع
            var opportunities = _context.Opportunities
                                        .Where(o => o.Type == type)
                                        .ToList(); // ⚡ جلب للذاكرة لتسهيل الفلترة النصية

            if (!string.IsNullOrEmpty(country))
            {
                // الفلترة في الذاكرة حسب الدولة
                opportunities = opportunities
                    .Where(o => !string.IsNullOrEmpty(o.AvailableCountries) &&
                                o.AvailableCountries
                                  .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                  .Select(c => c.Trim())
                                  .Contains(country.Trim(), StringComparer.OrdinalIgnoreCase))
                    .ToList();
            }

            // إنشاء الموديل لعرضه في الفيو
            var model = new AllOpportunitiesViewModel
            {
                Opportunities = opportunities,
                SelectedCountry = country,
                SelectedType = type
            };

            ViewBag.TypeName = typeName;
            return View("AllOpportunities", model);
        }
        // مثال لكل نوع:
        public IActionResult Trainings(string country = null)
        {
            return GetOpportunitiesByType(Foras_Khadra.Models.OpportunityType.Internships, country, "فرص التدريب");
        }

        public IActionResult Jobs(string country = null)
        {
            return GetOpportunitiesByType(Foras_Khadra.Models.OpportunityType.Jobs, country, "الوظائف");
        }

        public IActionResult Competitions(string country = null)
        {
            return GetOpportunitiesByType(Foras_Khadra.Models.OpportunityType.Competitions, country, "المسابقات");
        }

        public IActionResult Conferences(string country = null)
        {
            return GetOpportunitiesByType(Foras_Khadra.Models.OpportunityType.Conferences, country, "المؤتمرات");
        }

        public IActionResult Volunteering(string country = null)
        {
            return GetOpportunitiesByType(Foras_Khadra.Models.OpportunityType.Volunteering, country, "فرص التطوع");
        }

        public IActionResult Fellowships(string country = null)
        {
            return GetOpportunitiesByType(Foras_Khadra.Models.OpportunityType.Fellowships, country, "الزمالات");
        }

        public IActionResult Scholarships(string country = null)
        {
            return GetOpportunitiesByType(Foras_Khadra.Models.OpportunityType.Scholarships, country, "المنح الدراسية");
        }

        // ================= صفحة كل الفرص العامة =================
        public IActionResult AllOpportunities(string country, string type)
        {
            // جلب كل الفرص أولاً
            var allOpportunities = _context.Opportunities.ToList();

            // فلترة حسب النوع
            if (!string.IsNullOrEmpty(type) && Enum.TryParse<Foras_Khadra.Models.OpportunityType>(type, out var parsedType))
            {
                allOpportunities = allOpportunities
                    .Where(o => o.Type == parsedType)
                    .ToList();
            }

            // فلترة حسب الدولة
            if (!string.IsNullOrEmpty(country))
            {
                allOpportunities = allOpportunities
                    .Where(o => !string.IsNullOrEmpty(o.AvailableCountries) &&
                                o.AvailableCountries
                                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                    .Select(c => c.Trim())
                                    .Contains(country.Trim(), StringComparer.OrdinalIgnoreCase))
                    .ToList();
            }

            // توليد قائمة الدول للفلتر
            var countries = allOpportunities
                .SelectMany(o => o.AvailableCountries
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(c => c.Trim()))
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            // إنشاء الموديل
            var model = new AllOpportunitiesViewModel
            {
                Opportunities = allOpportunities,
                SelectedCountry = country,
                SelectedType = string.IsNullOrEmpty(type) ? null : Enum.Parse<Foras_Khadra.Models.OpportunityType>(type),
                Countries = countries
            };

            return View(model);
        }
    }
}
