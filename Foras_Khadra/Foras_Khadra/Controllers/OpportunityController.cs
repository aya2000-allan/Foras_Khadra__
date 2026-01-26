using Foras_Khadra.Data;
using Foras_Khadra.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;

namespace Foras_Khadra.Controllers
{
    public class OpportunityController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<ApplicationUser> _userManager;

        public OpportunityController(ApplicationDbContext context, IWebHostEnvironment env, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _env = env;
            _userManager = userManager; // صح
        }

        // ================= INDEX =================
        public IActionResult Index()
        {
            var opportunities = _context.Opportunities.ToList();

            // تجميع الفرص حسب النوع
            var grouped = opportunities
                .GroupBy(o => o.Type)
                .ToDictionary(g => g.Key, g => g.ToList());

            return View(grouped);
        }

        // ================= CREATE =================
        public async Task<IActionResult> Create()
        {
            var model = new OpportunityCreateVM();
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
        public async Task<IActionResult> Create(OpportunityCreateVM model)
        {
            if (!ModelState.IsValid) return View(model);

            string imagePath = null;
            if (model.Image != null)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(model.Image.FileName);
                var path = Path.Combine(_env.WebRootPath, "uploads/opportunities", fileName);
                using var stream = new FileStream(path, FileMode.Create);
                model.Image.CopyTo(stream);
                imagePath = "/uploads/opportunities/" + fileName;
            }

            var currentUser = await _userManager.GetUserAsync(User);
            var org = await _context.Organizations.FirstOrDefaultAsync(o => o.UserId == currentUser.Id);

            var opportunity = new Opportunity
            {
                Title = model.Title,
                ImagePath = imagePath,
                PublishedBy = org != null ? org.Name : "Admin",
                Type = model.Type.Value,
                Description = model.Description,
                Details = model.Details,
                AvailableCountries = model.AvailableCountries,
                EligibilityCriteria = model.EligibilityCriteria,
                Benefits = model.Benefits,
                ApplyLink = model.ApplyLink,
                CreatedByUserId = currentUser.Id,
                // تحديد التوثيق عند الإنشاء فقط
                IsPublishedByAdmin = await _userManager.IsInRoleAsync(currentUser, "Admin"),
                IsPublishedByOrganization = false
            };

            _context.Opportunities.Add(opportunity);
            await _context.SaveChangesAsync();

            if (await _userManager.IsInRoleAsync(currentUser, "Admin")) return RedirectToAction("Index");
            if (await _userManager.IsInRoleAsync(currentUser, "Organization")) return RedirectToAction("OrgOpportunities");

            return RedirectToAction("Index");
        }


        // ================= EDIT =================
        public IActionResult Edit(int id)
        {
            var opp = _context.Opportunities.Find(id);
            if (opp == null) return NotFound();

            var model = new OpportunityEditVM
            {
                Id = opp.Id,
                Title = opp.Title,
                PublishedBy = opp.PublishedBy,
                Type = opp.Type,
                Description = opp.Description,
                Details = opp.Details,
                AvailableCountries = opp.AvailableCountries,
                EligibilityCriteria = opp.EligibilityCriteria,
                Benefits = opp.Benefits,
                ApplyLink = opp.ApplyLink,
                ImagePath = opp.ImagePath // ← مهم لعرض الصورة الحالية
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(OpportunityEditVM model)
        {
            var old = _context.Opportunities.Find(model.Id);
            if (old == null) return NotFound();

            // تحديث الصورة إذا رفعت جديدة
            if (model.Image != null)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(model.Image.FileName);
                var path = Path.Combine(_env.WebRootPath, "uploads/opportunities", fileName);

                using var stream = new FileStream(path, FileMode.Create);
                model.Image.CopyTo(stream);

                old.ImagePath = "/uploads/opportunities/" + fileName;
            }

            // تحديث باقي الحقول فقط إذا تم تعديلها
            if (!string.IsNullOrEmpty(model.Title)) old.Title = model.Title;
            if (!string.IsNullOrEmpty(model.PublishedBy)) old.PublishedBy = model.PublishedBy;
            if (model.Type.HasValue) old.Type = model.Type.Value;
            if (!string.IsNullOrEmpty(model.Description)) old.Description = model.Description;
            if (!string.IsNullOrEmpty(model.Details)) old.Details = model.Details;
            if (!string.IsNullOrEmpty(model.AvailableCountries)) old.AvailableCountries = model.AvailableCountries;
            if (!string.IsNullOrEmpty(model.EligibilityCriteria)) old.EligibilityCriteria = model.EligibilityCriteria;
            if (!string.IsNullOrEmpty(model.Benefits)) old.Benefits = model.Benefits;
            if (!string.IsNullOrEmpty(model.ApplyLink)) old.ApplyLink = model.ApplyLink;

            // **تحديد الناشر تلقائياً حسب من أنشأ الفرصة**
            var creator = await _userManager.FindByIdAsync(old.CreatedByUserId);
            if (await _userManager.IsInRoleAsync(creator, "Organization"))
            {
                var org = await _context.Organizations.FirstOrDefaultAsync(o => o.UserId == creator.Id);
                old.PublishedBy = org?.Name;
            }
            else if (await _userManager.IsInRoleAsync(creator, "Admin"))
            {
                old.PublishedBy = "Admin";
            }
            _context.SaveChanges();
            // تحديد المستخدم الحالي
            var currentUser = await _userManager.GetUserAsync(User);

            // إعادة التوجيه حسب الدور
            if (await _userManager.IsInRoleAsync(currentUser, "Admin"))
            {
                return RedirectToAction("Index"); // صفحة الادمن العامة
            }
            else if (await _userManager.IsInRoleAsync(currentUser, "Organization"))
            {
                return RedirectToAction("OrgOpportunities"); // صفحة الفرص الخاصة بالمنظمة
            }

            // بشكل افتراضي
            return RedirectToAction("Index");
        }       

        // ================= DELETE =================
        public IActionResult Delete(int id)
        {
            var opp = _context.Opportunities.Find(id);
            if (opp != null)
            {
                _context.Opportunities.Remove(opp);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        // ================= DETAILS =================
        public async Task<IActionResult> Details(int id)
        {
            var opp = await _context.Opportunities
                .FirstOrDefaultAsync(o => o.Id == id);

            if (opp == null) return NotFound();

            var creator = await _userManager.FindByIdAsync(opp.CreatedByUserId);
            bool isAdminPublisher = false;
            if (creator != null)
            {
                isAdminPublisher = await _userManager.IsInRoleAsync(creator, "Admin");
            }

            // إنشاء ViewModel
            var model = new Opportunity
            {
                Id = opp.Id,
                Title = opp.Title,
                ImagePath = opp.ImagePath,
                Type = opp.Type,
                Description = opp.Description,
                Details = opp.Details,
                AvailableCountries = opp.AvailableCountries,
                EligibilityCriteria = opp.EligibilityCriteria,
                Benefits = opp.Benefits,
                ApplyLink = opp.ApplyLink,
                PublishedBy = opp.PublishedBy,
                PublishDate = opp.PublishDate,
                IsPublishedByAdmin = isAdminPublisher
            };

            return View(model);
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

        [Authorize(Roles = "Organization")]
        public async Task<IActionResult> OrgOpportunities()
        {
            var user = await _userManager.GetUserAsync(User);
            var org = await _context.Organizations.FirstOrDefaultAsync(o => o.UserId == user.Id);

            var opportunities = await _context.Opportunities
                .Where(o => o.CreatedByUserId == user.Id)
                .ToListAsync();

            var model = new OrgOpportunityPageVM
            {
                OrganizationName = org?.Name,
                Opportunities = opportunities.Select(o =>
                {
                    // جلب طلب الريلز الخاص بهذه الفرصة وهذه المنظمة
                    var request = _context.ReelsRequests
                                          .FirstOrDefault(r => r.OpportunityId == o.Id && r.OrganizationId == org.Id);

                    return new OrgOpportunityVM
                    {
                        Id = o.Id,
                        Title = o.Title,
                        Description = o.Description,
                        AvailableCountries = o.AvailableCountries,
                        Type = o.Type,
                        PublishDate = o.PublishDate,
                        ImagePath = o.ImagePath,

                        // حالة الريلز
                        HasRequestedReels = request != null,
                        IsReelsCompleted = request != null && request.IsCompleted,
                        IsReelsRejected = request != null && request.IsRejected,         // ✅ مرفوض
                        RejectionReason = request != null ? request.RejectionReason : "" // ✅ سبب الرفض
                    };
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Organization")]
        public async Task<IActionResult> ToggleReelsRequest(int id, [FromBody] ReelsRequestDto dto)
        {
            var user = await _userManager.GetUserAsync(User);
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
