using Foras_Khadra.Data;
using Foras_Khadra.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Azure.Core.HttpHeader;
using static System.Net.Mime.MediaTypeNames;

namespace Foras_Khadra.Controllers
{
    [Authorize]
    public class OpportunityController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<ApplicationUser> _userManager;

        public OpportunityController(ApplicationDbContext context, IWebHostEnvironment env, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _env = env;
            _userManager = userManager;
        }

        // ================= INDEX =================
        public IActionResult Index()
        {
            var opportunities = _context.Opportunities.ToList();

            var grouped = opportunities
                .GroupBy(o => o.Type)
                .ToDictionary(g => g.Key, g => g.ToList());

            return View(grouped);
        }

        // ================= CREATE =================
[HttpGet]
[Authorize(Roles = "Admin,Organization")]
public async Task<IActionResult> Create()
        {
            var model = new OpportunityCreateVM();
            var countries = await _context.Countries.ToListAsync();

            // احصل على لغة الموقع الحالية
            var culture = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

            model.CountriesSelectList = countries.Select(c => new SelectListItem
            {
                Text = culture switch
                {
                    "en" => c.NameEn,
                    "fr" => c.NameFr,
                    _ => c.NameAr
                },
                Value = c.Id.ToString(),
                Selected = model.AvailableCountryIds?.Contains(c.Id) ?? false
            }).ToList();


            var currentUser = await _userManager.GetUserAsync(User);

            if (await _userManager.IsInRoleAsync(currentUser, "Organization"))
            {
                var org = await _context.Organizations.FirstOrDefaultAsync(o => o.UserId == currentUser.Id);
                if (org != null) model.PublishedBy = org.Name;
            }
            else if (await _userManager.IsInRoleAsync(currentUser, "Admin"))
            {
                model.PublishedBy = "Admin";
            }

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Organization")]
        public async Task<IActionResult> Create(OpportunityCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                // إعادة ملء قائمة الدول حسب لغة الموقع
                var countries = await _context.Countries.ToListAsync();
                var culture = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

                model.CountriesSelectList = countries.Select(c => new SelectListItem
                {
                    Text = culture switch
                    {
                        "en" => c.NameEn,
                        "fr" => c.NameFr,
                        _ => c.NameAr
                    },
                    Value = c.Id.ToString(),
                    Selected = model.AvailableCountryIds?.Contains(c.Id) ?? false
                }).ToList();

                return View(model);
            }

            string imagePath = null;
            if (model.Image != null)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(model.Image.FileName);
                var path = Path.Combine(_env.WebRootPath, "uploads/opportunities", fileName);
                using var stream = new FileStream(path, FileMode.Create);
                await model.Image.CopyToAsync(stream);
                imagePath = "/uploads/opportunities/" + fileName;
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return RedirectToAction("Login", "Account");

            var org = await _context.Organizations.FirstOrDefaultAsync(o => o.UserId == currentUser.Id);

            var opportunity = new Opportunity
            {
                TitleAr = model.TitleAr,
                TitleEn = model.TitleEn,
                TitleFr = model.TitleFr,
                DescriptionAr = model.DescriptionAr,
                DescriptionEn = model.DescriptionEn,
                DescriptionFr = model.DescriptionFr,
                DetailsAr = model.DetailsAr,
                DetailsEn = model.DetailsEn,
                DetailsFr = model.DetailsFr,
                AvailableCountries = model.AvailableCountryIds != null
                    ? await _context.Countries
                        .Where(c => model.AvailableCountryIds.Contains(c.Id))
                        .ToListAsync()
                    : new List<Country>(),
                EligibilityCriteriaAr = model.EligibilityCriteriaAr,
                EligibilityCriteriaEn = model.EligibilityCriteriaEn,
                EligibilityCriteriaFr = model.EligibilityCriteriaFr,
                BenefitsAr = model.BenefitsAr,
                BenefitsEn = model.BenefitsEn,
                BenefitsFr = model.BenefitsFr,
                ApplyLink = model.ApplyLink,
                ImagePath = imagePath,
                PublishedBy = org != null ? org.Name : "Admin",
                Type = model.Type.Value,
                CreatedByUserId = currentUser.Id,
                IsPublishedByAdmin = await _userManager.IsInRoleAsync(currentUser, "Admin"),
                IsPublishedByOrganization = await _userManager.IsInRoleAsync(currentUser, "Organization")
            };

            _context.Opportunities.Add(opportunity);
            await _context.SaveChangesAsync();

            return RedirectToAction(await _userManager.IsInRoleAsync(currentUser, "Admin") ? "Index" : "OrgOpportunities");
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Organization")]
        public async Task<IActionResult> Edit(OpportunityEditVM model)
        {
            // أولاً تحقق من صحة الموديل
            if (!ModelState.IsValid)
            {
                var countries = await _context.Countries.ToListAsync();
                var culture = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

                model.CountriesSelectList = countries.Select(c => new SelectListItem
                {
                    Text = culture switch
                    {
                        "en" => c.NameEn,
                        "fr" => c.NameFr,
                        _ => c.NameAr
                    },
                    Value = c.Id.ToString(),
                    Selected = model.AvailableCountryIds?.Contains(c.Id) ?? false
                }).ToList();

                return View(model);
            }

            // جلب المستخدم الحالي
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return RedirectToAction("Login", "Account");

            var org = await _context.Organizations.FirstOrDefaultAsync(o => o.UserId == currentUser.Id);

            // جلب الفرصة الحالية بدل إنشاء واحدة جديدة
            var opportunity = await _context.Opportunities
                .Include(o => o.AvailableCountries)
                .FirstOrDefaultAsync(o => o.Id == model.Id);

            if (opportunity == null) return NotFound();

            if (opportunity.CreatedByUserId != currentUser.Id && !await _userManager.IsInRoleAsync(currentUser, "Admin"))
                return Forbid();

            // تحديث الحقول
            opportunity.TitleAr = model.TitleAr;
            opportunity.TitleEn = model.TitleEn;
            opportunity.TitleFr = model.TitleFr;
            opportunity.DescriptionAr = model.DescriptionAr;
            opportunity.DescriptionEn = model.DescriptionEn;
            opportunity.DescriptionFr = model.DescriptionFr;
            opportunity.DetailsAr = model.DetailsAr;
            opportunity.DetailsEn = model.DetailsEn;
            opportunity.DetailsFr = model.DetailsFr;
            opportunity.EligibilityCriteriaAr = model.EligibilityCriteriaAr;
            opportunity.EligibilityCriteriaEn = model.EligibilityCriteriaEn;
            opportunity.EligibilityCriteriaFr = model.EligibilityCriteriaFr;
            opportunity.BenefitsAr = model.BenefitsAr;
            opportunity.BenefitsEn = model.BenefitsEn;
            opportunity.BenefitsFr = model.BenefitsFr;
            opportunity.ApplyLink = model.ApplyLink;
            opportunity.Type = model.Type.Value;

            // تحديث الصورة فقط إذا تم رفعها
            if (model.Image != null)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(model.Image.FileName);
                var path = Path.Combine(_env.WebRootPath, "uploads/opportunities", fileName);
                using var stream = new FileStream(path, FileMode.Create);
                await model.Image.CopyToAsync(stream);
                opportunity.ImagePath = "/uploads/opportunities/" + fileName;
            }

            // تحديث الدول المتاحة
            if (model.AvailableCountryIds != null)
            {
                opportunity.AvailableCountries = await _context.Countries
                    .Where(c => model.AvailableCountryIds.Contains(c.Id))
                    .ToListAsync();
            }
            else
            {
                opportunity.AvailableCountries = new List<Country>();
            }

            // تحديث من نشر
            opportunity.PublishedBy = org != null ? org.Name : "Admin";
            opportunity.IsPublishedByAdmin = await _userManager.IsInRoleAsync(currentUser, "Admin");
            opportunity.IsPublishedByOrganization = await _userManager.IsInRoleAsync(currentUser, "Organization");

            var existingRequest = await _context.ReelsRequests
    .FirstOrDefaultAsync(r => r.OpportunityId == opportunity.Id);

            if (existingRequest != null && existingRequest.IsRejected)
            {
                existingRequest.IsRejected = false;
                existingRequest.IsCompleted = false;
                existingRequest.IsInProgress = false;
                existingRequest.RejectionReason = null;
                existingRequest.RequestDate = DateTime.Now; // ترجع Pending تلقائياً
            }
            // حفظ التغييرات
            _context.Opportunities.Update(opportunity);
            await _context.SaveChangesAsync();

            // إعادة التوجيه حسب الدور
            bool isAdmin = await _userManager.IsInRoleAsync(currentUser, "Admin");
            return RedirectToAction(isAdmin ? "Index" : "OrgOpportunities");
        }


        // ================= EDIT GET =================
        [Authorize(Roles = "Admin,Organization")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var opp = await _context.Opportunities
                .Include(o => o.AvailableCountries)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (opp == null) return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return RedirectToAction("Login", "Account");

            if (opp.CreatedByUserId != currentUser.Id && !await _userManager.IsInRoleAsync(currentUser, "Admin"))
                return Forbid();

            var countries = await _context.Countries.ToListAsync();
            var culture = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

            var model = new OpportunityEditVM
            {
                Id = opp.Id,
                TitleAr = opp.TitleAr,
                TitleEn = opp.TitleEn,
                TitleFr = opp.TitleFr,
                PublishedBy = opp.PublishedBy,
                Type = opp.Type,
                DescriptionAr = opp.DescriptionAr,
                DescriptionEn = opp.DescriptionEn,
                DescriptionFr = opp.DescriptionFr,
                DetailsAr = opp.DetailsAr,
                DetailsEn = opp.DetailsEn,
                DetailsFr = opp.DetailsFr,
                AvailableCountryIds = opp.AvailableCountries?.Select(c => c.Id).ToList() ?? new List<int>(),
                CountriesSelectList = countries.Select(c => new SelectListItem
                {
                    Text = culture switch
                    {
                        "en" => c.NameEn,
                        "fr" => c.NameFr,
                        _ => c.NameAr
                    },
                    Value = c.Id.ToString(),
                    Selected = opp.AvailableCountries?.Any(ac => ac.Id == c.Id) ?? false
                }).ToList(),
                BenefitsAr = opp.BenefitsAr,
                BenefitsEn = opp.BenefitsEn,
                BenefitsFr = opp.BenefitsFr,
                EligibilityCriteriaAr = opp.EligibilityCriteriaAr,
                EligibilityCriteriaEn = opp.EligibilityCriteriaEn,
                EligibilityCriteriaFr = opp.EligibilityCriteriaFr,
                ApplyLink = opp.ApplyLink,
                ImagePath = opp.ImagePath
            };

            return View(model);
        }




        // ================= DELETE =================
        [Authorize(Roles = "Admin,Organization")]
        public async Task<IActionResult> Delete(int id)
        {
            var opp = await _context.Opportunities.FindAsync(id);
            if (opp == null) return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return RedirectToAction("Login", "Account");

            if (opp.CreatedByUserId != currentUser.Id && !await _userManager.IsInRoleAsync(currentUser, "Admin"))
                return Forbid();

            _context.Opportunities.Remove(opp);
            await _context.SaveChangesAsync();

            return RedirectToAction(await _userManager.IsInRoleAsync(currentUser, "Admin") ? "Index" : "OrgOpportunities");
        }

        // ================= DETAILS =================
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var opp = await _context.Opportunities
                .Include(o => o.AvailableCountries)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (opp == null) return NotFound();

            var creator = await _userManager.FindByIdAsync(opp.CreatedByUserId);
            opp.IsPublishedByAdmin = creator != null && await _userManager.IsInRoleAsync(creator, "Admin");

            return View(opp);
        }

        // ================= FILTER BY TYPE =================
        [Route("Opportunities/{type}")]
        public IActionResult ByType(OpportunityType type)
        {
            var data = _context.Opportunities
                .Where(x => x.Type == type)
                .ToList();

            ViewBag.Type = type;
            return View(data);
        }

        // ================= ORG OPPORTUNITIES =================
        [Authorize(Roles = "Organization")]
        public async Task<IActionResult> OrgOpportunities()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var org = await _context.Organizations.FirstOrDefaultAsync(o => o.UserId == user.Id);
            var opportunities = await _context.Opportunities
    .Where(o => o.CreatedByUserId == user.Id)
    .Include(o => o.AvailableCountries) 
    .ToListAsync();

            var model = new OrgOpportunityPageVM
            {
                OrganizationName = org?.Name,
                Opportunities = opportunities.Select(o =>
                {
                    var request = _context.ReelsRequests
                                          .FirstOrDefault(r => r.OpportunityId == o.Id && r.OrganizationId == org.Id);

                    return new OrgOpportunityVM
                    {
                        Id = o.Id,
                        TitleAr = o.TitleAr,
                        TitleEn = o.TitleEn,
                        TitleFr = o.TitleFr,
                        DescriptionAr = o.DescriptionAr,
                        DescriptionEn = o.DescriptionEn,
                        DescriptionFr = o.DescriptionFr,
                        AvailableCountries = o.AvailableCountries?.ToList() ?? new List<Country>(),
                        Type = o.Type,
                        PublishDate = o.PublishDate,
                        ImagePath = o.ImagePath,
                        HasRequestedReels = request != null,
                        IsReelsCompleted = request?.IsCompleted ?? false,
                        IsReelsRejected = request?.IsRejected ?? false,
                        RejectionReason = request?.RejectionReason ?? "",
                        IsReelsInProgress = request?.IsInProgress ?? false
                    };
                }).ToList()
            };

            return View(model);
        }

        // ================= TOGGLE REELS REQUEST =================
        [HttpPost]
        [Authorize(Roles = "Organization")]
        public async Task<IActionResult> ToggleReelsRequest(int id, [FromBody] ReelsRequestDto dto)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return BadRequest();

            var org = await _context.Organizations.FirstOrDefaultAsync(o => o.UserId == user.Id);
            if (org == null) return BadRequest();

            var request = await _context.ReelsRequests
                .FirstOrDefaultAsync(r => r.OpportunityId == id && r.OrganizationId == org.Id);

            if (dto.RequestReels)
            {
                if (request == null)
                {
                    request = new ReelsRequest
                    {
                        OpportunityId = id,
                        OrganizationId = org.Id,
                        RequestDate = DateTime.Now,
                        IsCompleted = false
                    };
                    _context.ReelsRequests.Add(request);
                }
            }
            else
            {
                if (request != null)
                {
                    _context.ReelsRequests.Remove(request);
                }
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        public class ReelsRequestDto
        {
            public bool RequestReels { get; set; }
        }


    }
}
