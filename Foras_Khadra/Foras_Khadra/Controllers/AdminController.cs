using Foras_Khadra.Data;
using Foras_Khadra.Models;
using Foras_Khadra.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Threading.Tasks;
using System.Globalization;

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
                RejectReelsRequests = rejectedReelsRequests
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


        [HttpPost]
        public async Task<IActionResult> UploadLogo(int id, IFormFile logoFile)
        {
            var org = await _context.Organizations.FindAsync(id);
            if (org == null) return NotFound();

            if (logoFile != null && logoFile.Length > 0)
            {
                // ✅ التحقق من نوع الملف
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                var ext = Path.GetExtension(logoFile.FileName).ToLower();

                if (!allowedExtensions.Contains(ext))
                {
                    return BadRequest("فقط صور مسموحة");
                }

                //  حذف القديم (إذا موجود)
                if (!string.IsNullOrEmpty(org.LogoPhotoPath))
                {
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", org.LogoPhotoPath.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }

                // حفظ الجديد
                var fileName = Guid.NewGuid().ToString() + ext;
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await logoFile.CopyToAsync(stream);
                }

                org.LogoPhotoPath = "/uploads/" + fileName;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("OrganizationDetails", new { id });
        }


        [HttpPost]
        public async Task<IActionResult> DeleteLogo(int id)
        {
            var org = await _context.Organizations.FindAsync(id);
            if (org == null) return NotFound();

            if (!string.IsNullOrEmpty(org.LogoPhotoPath))
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", org.LogoPhotoPath.TrimStart('/'));

                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

                org.LogoPhotoPath = null;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("OrganizationDetails", new { id });
        }
    }


        
}
