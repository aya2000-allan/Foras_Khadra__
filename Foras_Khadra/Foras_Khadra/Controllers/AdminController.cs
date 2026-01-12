using Foras_Khadra.Data;
using Foras_Khadra.ViewModels;
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
            var model = new AdminDashboardViewModel
            {
                TotalOrganizations = await _context.Organizations.CountAsync(),
                TotalUsers = await _context.Users.CountAsync(u => u.Role == Models.UserRole.User),
                TotalOpportunities = await _context.Opportunities.CountAsync(),
                TotalArticles = await _context.Articles.CountAsync()
            };

            return View(model);
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
