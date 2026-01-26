using Foras_Khadra.Data;
using Foras_Khadra.Models;
using Foras_Khadra.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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
                TotalUsers = await _context.Users.CountAsync(u => u.Role == UserRole.User),
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
            var requests = _context.ReelsRequests
                .Include(r => r.Organization)
                .Include(r => r.Opportunity)
                .Select(r => new ReelsRequestAdminVM
                {
                    Id = r.Id,
                    OpportunityId = r.OpportunityId,
                    OpportunityTitle = r.Opportunity.Title,
                    OpportunityType = r.Opportunity.Type,
                    OrganizationName = r.Organization.Name,
                    RequestDate = r.RequestDate,
                    IsCompleted = r.IsCompleted,
                    IsRejected = r.IsRejected,
                    IsInProgress = r.IsInProgress, // حالة جاري العمل عليها
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

    }


        
}
