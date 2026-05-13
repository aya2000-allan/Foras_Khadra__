using Foras_Khadra.Data;
using Foras_Khadra.Models;
using Foras_Khadra.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using System.IO;
namespace Foras_Khadra.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Dashboard()
        {
            var totalReelsRequests = await _context.ReelsRequests.CountAsync();
            var pendingReelsRequests = await _context.ReelsRequests.CountAsync(r => r.IsInProgress);
            var completedReelsRequests = await _context.ReelsRequests.CountAsync(r => r.IsCompleted);
            var rejectedReelsRequests = await _context.ReelsRequests.CountAsync(r => r.IsRejected);

            var model = new AdminDashboardViewModel
            {
                TotalOrganizations = await _context.Organizations.CountAsync(),
                TotalUsers = await _context.TeamMember.CountAsync(),
                TotalRegisteredUsers = await _context.Users.CountAsync(u => u.Role == UserRole.User),
                TotalOpportunities = await _context.Opportunities.CountAsync(),
                TotalArticles = await _context.Articles.CountAsync(),
                TotalReelsRequests = totalReelsRequests,
                PendingReelsRequests = pendingReelsRequests,
                CompletedReelsRequests = completedReelsRequests,
                RejectReelsRequests = rejectedReelsRequests,
                TotalManualOrganizations = await _context.ManualOrganizations.CountAsync()
            };

            return View(model);
        }

        public IActionResult ReelsRequestsDashboard(string status = null)
        {
            var lang = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            var requests = _context.ReelsRequests
                .Include(r => r.Organization)
                .Include(r => r.Opportunity)
                .Select(r => new ReelsRequestAdminVM
                {
                    Id = r.Id,
                    OpportunityId = r.OpportunityId,
                    OpportunityTitle = lang == "en" ? r.Opportunity.TitleEn :
                               lang == "fr" ? r.Opportunity.TitleFr :
                               r.Opportunity.TitleAr,
                    OpportunityType = r.Opportunity.Type,
                    OrganizationName = r.Organization.Name,
                    RequestDate = r.RequestDate,
                    IsCompleted = r.IsCompleted,
                    IsRejected = r.IsRejected,
                    IsInProgress = r.IsInProgress,

                    RejectionReason = r.RejectionReason
                })
                .ToList();

            if (!string.IsNullOrEmpty(status))
            {
                status = status.ToLower();
                if (status == "pending")
                    requests = requests.Where(r => r.IsInProgress).ToList(); // فقط الطلبات التي حددها الأدمن
                else if (status == "completed")
                    requests = requests.Where(r => r.IsCompleted).ToList();
                else if (status == "rejected")
                    requests = requests.Where(r => r.IsRejected).ToList();
            }

            return View(requests);
        }


        [HttpPost]
        public async Task<IActionResult> ToggleReelsRequest(int id, string newStatus, string? rejectionReason)
        {
            var request = await _context.ReelsRequests.FindAsync(id);
            if (request == null) return NotFound();

            // تعديل الحالات حسب اختيار الادمن
            request.IsCompleted = newStatus == "completed";
            request.IsRejected = newStatus == "rejected";

            //  حالة جاري العمل عليها فقط إذا الادمن اختار "pending"
            request.IsInProgress = newStatus == "pending";

            // حفظ سبب الرفض إذا تم رفض الطلب
            request.RejectionReason = request.IsRejected ? rejectionReason : null;

            await _context.SaveChangesAsync();
            return Ok();
        }


        public async Task<IActionResult> OrganizationsList()
        {
            var organizations = await _context.Organizations.ToListAsync();
            return View(organizations);
        }

        public async Task<IActionResult> OrganizationDetails(int id)
        {
            var org = await _context.Organizations.FindAsync(id);
            if (org == null) return NotFound();
            return View(org);
        }

        [HttpGet]
        public async Task<IActionResult> EditOrganization(int id)
        {
            var org = await _context.Organizations.FindAsync(id);
            if (org == null) return NotFound();

            return View(org);
        }

        [HttpPost]
        public async Task<IActionResult> EditOrganization(int id, Organization formModel, IFormFile logoFile)
        {
            var org = await _context.Organizations.FindAsync(id);

            if (org == null)
                return NotFound();

            // رفع اللوجو
            if (logoFile != null && logoFile.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(logoFile.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/logos", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await logoFile.CopyToAsync(stream);
                }

                org.LogoPath = "/logos/" + fileName;
            }

            // تحديث البيانات من formModel (مش model)
            org.Name = formModel.Name ?? org.Name;
            org.Sector = formModel.Sector ?? org.Sector;
            org.Country = formModel.Country ?? org.Country;
            org.ContactName = formModel.ContactName ?? org.ContactName;
            org.ContactEmail = formModel.ContactEmail ?? org.ContactEmail;
            org.PhoneNumber = formModel.PhoneNumber ?? org.PhoneNumber;
            org.Location = formModel.Location ?? org.Location;
            org.Website = formModel.Website ?? org.Website;

            await _context.SaveChangesAsync();

            return RedirectToAction("OrganizationsList");
        }


        //--------------------------//

        public async Task<IActionResult> ManualOrganizationsList()
        {
            var organizations = await _context.ManualOrganizations
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            return View(organizations);
        }

        [HttpGet]
        public IActionResult AddManualOrganization()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddManualOrganization(ManualOrganization model, IFormFile logoFile)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (logoFile != null && logoFile.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(logoFile.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/logos", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await logoFile.CopyToAsync(stream);
                }

                model.LogoPath = "/logos/" + fileName;
            }

            _context.ManualOrganizations.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("ManualOrganizationsList");
        }

        [HttpGet]
        public async Task<IActionResult> EditManualOrganization(int id)
        {
            var org = await _context.ManualOrganizations.FindAsync(id);

            if (org == null)
                return NotFound();

            return View(org);
        }

        [HttpPost]
        public async Task<IActionResult> EditManualOrganization(ManualOrganization model, IFormFile logoFile)
        {
            var org = await _context.ManualOrganizations.FindAsync(model.Id);

            if (org == null)
                return NotFound();

            if (logoFile != null && logoFile.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(logoFile.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/logos", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await logoFile.CopyToAsync(stream);
                }

                org.LogoPath = "/logos/" + fileName;
            }

            org.OrganizationName = model.OrganizationName;
            org.ContactPersonName = model.ContactPersonName;
            org.Details = model.Details;
            org.PhoneNumber = model.PhoneNumber;
            org.Email = model.Email;
            org.Website = model.Website;
            org.Location = model.Location;
            org.Country = model.Country;
            org.Sector = model.Sector;
            org.IsActive = model.IsActive;


            await _context.SaveChangesAsync();

            return RedirectToAction("ManualOrganizationsList");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteManualOrganization(int id)
        {
            var org = await _context.ManualOrganizations.FindAsync(id);

            if (org == null)
                return NotFound();

            _context.ManualOrganizations.Remove(org);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ManualOrganizationsList));
        }


        public async Task<IActionResult> ManualOrganizationDetails(int id)
        {
            var org = await _context.ManualOrganizations.FindAsync(id);

            if (org == null)
                return NotFound();

            return View(org);
        }




    }



}
