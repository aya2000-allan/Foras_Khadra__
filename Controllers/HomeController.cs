using System.Globalization;
using Foras_Khadra.Data;
using Foras_Khadra.Helpers;
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

            var organizations = _context.Organizations
       .Where(o => !string.IsNullOrEmpty(o.LogoPath)) 
       .Select(o => new Organization
       {
           Id = o.Id,
           Name = o.Name,
           LogoPath = o.LogoPath
       })
       .ToList();

            var model = new HomeViewModel
            {
                LatestArticles = latestArticles,
                LatestOpportunities = latestOpportunities,
                Organizations = organizations

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

            // ================== إضافة حالة الانتهاء ==================
            foreach (var opp in opportunities)
            {
                opp.IsExpired = opp.EndDate.HasValue && opp.EndDate.Value < DateTime.Now;
            }
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
        public IActionResult AllOpportunities(string country, string[] type, string search)
        {
            var query = _context.Opportunities
                                .Include(o => o.AvailableCountries)
                                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(o =>
                    EF.Functions.Like(o.TitleAr, $"%{search}%") ||
                    EF.Functions.Like(o.TitleEn, $"%{search}%") ||
                    EF.Functions.Like(o.TitleFr, $"%{search}%"));
            }

            // فلترة حسب النوع
            if (type != null && type.Any())
            {
                // تحويل النصوص إلى Enum
                var selectedTypes = type
                    .Where(t => Enum.TryParse<OpportunityType>(t, out _))
                    .Select(t => Enum.Parse<OpportunityType>(t))
                    .ToList();

                query = query.Where(o => o.Type.HasValue && selectedTypes.Contains(o.Type.Value));
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

            var allOpportunities = query
                .OrderByDescending(o => o.PublishDate)   // من الأحدث للأقدم
                .ToList();
            // ================== إضافة حالة الانتهاء ==================
            foreach (var opp in allOpportunities)
            {
                opp.IsExpired = opp.EndDate.HasValue && opp.EndDate.Value < DateTime.Now;
            }
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


        public IActionResult OrganizationsMap(string country = null, string sector = null, int page = 1)
        {
            int pageSize = 24;

            var query = _context.Organizations
                .Where(o => !string.IsNullOrEmpty(o.LogoPath))
                .AsQueryable();

            if (!string.IsNullOrEmpty(country))
                query = query.Where(o => o.Country == country);

            if (!string.IsNullOrEmpty(sector))
            {
                query = query.AsEnumerable()
                    .Where(o =>
                    {
                        if (string.IsNullOrWhiteSpace(o.Sector))
                            return false;

                        var s1 = o.Sector.Trim().ToLower();
                        var s2 = sector.Trim().ToLower();

                        s1 = s1
                            .Replace("ة", "ه")
                            .Replace("أ", "ا")
                            .Replace("إ", "ا")
                            .Replace("آ", "ا")
                            .Replace("ى", "ي");

                        s2 = s2
                            .Replace("ة", "ه")
                            .Replace("أ", "ا")
                            .Replace("إ", "ا")
                            .Replace("آ", "ا")
                            .Replace("ى", "ي");

                        s1 = new string(s1.Where(c => char.IsLetter(c) || char.IsWhiteSpace(c)).ToArray());
                        s2 = new string(s2.Where(c => char.IsLetter(c) || char.IsWhiteSpace(c)).ToArray());

                        s1 = string.Join(" ", s1.Split(' ', StringSplitOptions.RemoveEmptyEntries));
                        s2 = string.Join(" ", s2.Split(' ', StringSplitOptions.RemoveEmptyEntries));

                        return s1 == s2;
                    })
                    .AsQueryable();
            }

            int totalItems = query.Count();

            var organizations = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.SelectedCountry = country;
            ViewBag.SelectedSector = sector;

            ViewBag.Countries = _context.Organizations
    .Where(o => !string.IsNullOrWhiteSpace(o.Country))
    .AsEnumerable()
    .GroupBy(o => o.Country.Trim().ToLower())
    .Select(g => g.First().Country.Trim())
    .OrderBy(c => c)
    .ToList();


            ViewBag.Sectors = _context.Organizations
    .Where(o => !string.IsNullOrWhiteSpace(o.Sector))
    .AsEnumerable()
    .Where(o =>
        !string.IsNullOrWhiteSpace(o.Sector) &&
        !o.Sector.Contains("@") &&
        o.Sector.All(c => char.IsLetter(c) || char.IsWhiteSpace(c))
    )
    .GroupBy(o =>
    {
        var s = o.Sector.Trim().ToLower();

        s = s
            .Replace("ة", "ه")
            .Replace("أ", "ا")
            .Replace("إ", "ا")
            .Replace("آ", "ا")
            .Replace("ى", "ي");

        s = new string(s.Where(c => char.IsLetter(c) || char.IsWhiteSpace(c)).ToArray());

        return string.Join(" ", s.Split(' ', StringSplitOptions.RemoveEmptyEntries));
    })
    .Select(g => g.First().Sector.Trim())
    .OrderBy(s => s)
    .ToList();

            return View(organizations);
        }


        public IActionResult DetailsOrganization(int id)
        {
            var org = _context.Organizations.FirstOrDefault(o => o.Id == id);

            if (org == null)
                return NotFound();

            return View(org);
        }
        
    }
}
