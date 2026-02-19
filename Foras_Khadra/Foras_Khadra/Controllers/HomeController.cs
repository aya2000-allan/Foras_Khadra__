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

            var latestOpportunities = _context.Opportunities
    .OrderByDescending(o => o.PublishDate)
    .Take(9)
    .AsNoTracking()
    .ToList();


            var model = new HomeViewModel
            {
                LatestArticles = latestArticles,
                LatestOpportunities = latestOpportunities
            };


            return View(model);
        }

        public IActionResult News()
        {
            return View();
        }

        public IActionResult PolicyPrivacy()
        {
            return View();
        }

        public IActionResult TermsAndConditions()
        {
            return View();
        }

        public IActionResult UsagePolicy()
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
        private IActionResult GetOpportunitiesByType(OpportunityType type, string country, string typeName)
        {
            var query = _context.Opportunities
                                .Include(o => o.AvailableCountries)
                                .Where(o => o.Type == type);

            if (!string.IsNullOrEmpty(country))
            {
                query = query.Where(o =>
                    o.AvailableCountries.Any(c =>
                        c.NameEn == country ||
                        c.NameAr == country ||
                        c.NameFr == country));
            }

            var opportunities = query.ToList();

            var model = new AllOpportunitiesViewModel
            {
                Opportunities = opportunities,
                SelectedCountry = country,
                SelectedTypes = new List<OpportunityType> { type } 
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
        public IActionResult AllOpportunities(string country, string[] type)
        {
            var query = _context.Opportunities
                                .Include(o => o.AvailableCountries)
                                .AsQueryable();

            // فلترة حسب النوع
            if (type != null && type.Any())
            {
                // تحويل النصوص إلى Enum
                var selectedTypes = type
                    .Where(t => Enum.TryParse<OpportunityType>(t, out _))
                    .Select(t => Enum.Parse<OpportunityType>(t))
                    .ToList();

                query = query.Where(o => selectedTypes.Contains(o.Type));
            }

            // فلترة حسب الدولة
            if (!string.IsNullOrEmpty(country))
            {
                query = query.Where(o =>
                    o.AvailableCountries.Any(c =>
                        c.NameEn == country ||
                        c.NameAr == country ||
                        c.NameFr == country));
            }

            var allOpportunities = query.ToList();

            var countries = _context.Countries
                                    .OrderBy(c => c.NameAr)
                                    .Select(c => c.NameAr)
                                    .ToList();

            var model = new AllOpportunitiesViewModel
            {
                Opportunities = allOpportunities,
                SelectedCountry = country,
                SelectedTypes = type?.Select(t => Enum.Parse<OpportunityType>(t)).ToList() ?? new List<OpportunityType>(),
                Countries = countries
            };

            return View(model);
        }

    }
}
