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
        [Authorize(Roles = "Admin,Organization")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new OpportunityCreateVM();

            // جلب الدول من الـ Database
            var countries = await _context.Countries.ToListAsync();
            model.CountriesSelectList = countries.Select(c => new SelectListItem
            {
                Text = c.NameAr,    // يمكن التبديل لـ NameEn أو NameFr حسب لغة الموقع
                Value = c.Id.ToString()
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

        [Authorize(Roles = "Admin,Organization")]
        [HttpPost]
        public async Task<IActionResult> Create(OpportunityCreateVM model)
        {
            // إذا كان النموذج غير صالح، أعيدي ملء قائمة الدول
            if (!ModelState.IsValid)
            {
                var countries = await _context.Countries.ToListAsync();
                model.CountriesSelectList = countries.Select(c => new SelectListItem
                {
                    Text = c.NameAr,
                    Value = c.Id.ToString()
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
                AvailableCountries = await _context.Countries
                            .Where(c => model.AvailableCountryIds.Contains(c.Id))
                            .ToListAsync(),
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

        // ================= EDIT =================
        [Authorize(Roles = "Admin,Organization")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var opp = await _context.Opportunities.FindAsync(id);
            if (opp == null) return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return RedirectToAction("Login", "Account");

            // تحقق أن المستخدم هو منشئ الفرصة أو Admin
            if (opp.CreatedByUserId != currentUser.Id && !await _userManager.IsInRoleAsync(currentUser, "Admin"))
                return Forbid();

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
                AvailableCountryIds = opp.AvailableCountries.Select(c => c.Id).ToList(),
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

        [Authorize(Roles = "Admin,Organization")]
        [HttpPost]
        public async Task<IActionResult> Edit(OpportunityEditVM model)
        {
            var old = await _context.Opportunities.FindAsync(model.Id);
            if (old == null) return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return RedirectToAction("Login", "Account");

            // تحقق من حقوق التعديل
            if (old.CreatedByUserId != currentUser.Id && !await _userManager.IsInRoleAsync(currentUser, "Admin"))
                return Forbid();

            if (model.Image != null)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(model.Image.FileName);
                var path = Path.Combine(_env.WebRootPath, "uploads/opportunities", fileName);
                using var stream = new FileStream(path, FileMode.Create);
                await model.Image.CopyToAsync(stream);
                old.ImagePath = "/uploads/opportunities/" + fileName;
            }

            old.TitleAr = model.TitleAr ?? old.TitleAr;
            old.TitleEn = model.TitleEn ?? old.TitleEn;
            old.TitleFr = model.TitleFr ?? old.TitleFr;
            old.PublishedBy = model.PublishedBy ?? old.PublishedBy;
            old.Type = model.Type ?? old.Type;
            old.DescriptionAr = model.DescriptionAr ?? old.DescriptionAr;
            old.DescriptionEn = model.DescriptionEn ?? old.DescriptionEn;
            old.DescriptionFr = model.DescriptionFr ?? old.DescriptionFr;
            old.DetailsAr = model.DetailsAr ?? old.DetailsAr;
            old.DetailsEn = model.DetailsEn ?? old.DetailsEn;
            old.DetailsFr = model.DetailsFr ?? old.DetailsFr;
            if (model.AvailableCountryIds?.Count > 0)
                old.AvailableCountries = await _context.Countries
                                            .Where(c => model.AvailableCountryIds.Contains(c.Id))
                                            .ToListAsync();
            old.EligibilityCriteriaAr = model.EligibilityCriteriaAr ?? old.EligibilityCriteriaAr;
            old.EligibilityCriteriaEn = model.EligibilityCriteriaEn ?? old.EligibilityCriteriaEn;
            old.EligibilityCriteriaFr = model.EligibilityCriteriaFr ?? old.EligibilityCriteriaFr;
            old.BenefitsAr = model.BenefitsAr ?? old.BenefitsAr;
            old.BenefitsEn = model.BenefitsEn ?? old.BenefitsEn;
            old.BenefitsFr = model.BenefitsFr ?? old.BenefitsFr;
            old.ApplyLink = model.ApplyLink ?? old.ApplyLink;

            // تحديث حالة الريلز إذا موجود
            var orgCurrent = await _context.Organizations.FirstOrDefaultAsync(o => o.UserId == currentUser.Id);
            if (orgCurrent != null)
            {
                var reelsRequest = await _context.ReelsRequests
                    .FirstOrDefaultAsync(r => r.OpportunityId == old.Id && r.OrganizationId == orgCurrent.Id);

                if (reelsRequest != null && reelsRequest.IsRejected)
                {
                    reelsRequest.IsRejected = false;
                    reelsRequest.RejectionReason = null;
                    reelsRequest.IsCompleted = false;
                    reelsRequest.IsInProgress = false;
                    reelsRequest.RequestDate = DateTime.Now;
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(await _userManager.IsInRoleAsync(currentUser, "Admin") ? "Index" : "OrgOpportunities");
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
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var opp = await _context.Opportunities
                .Include(o => o.AvailableCountries)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (opp == null) return NotFound();

            var creator = await _userManager.FindByIdAsync(opp.CreatedByUserId);
            bool isAdminPublisher = creator != null && await _userManager.IsInRoleAsync(creator, "Admin");

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
                        AvailableCountryNames = o.AvailableCountries.Select(c => c.NameAr).ToList(),
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
