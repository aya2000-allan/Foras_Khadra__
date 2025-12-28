using Foras_Khadra.Data;
using Foras_Khadra.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;

namespace Foras_Khadra.Controllers
{
    public class TeamMembersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TeamMembersController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Index
        public IActionResult Index()
        {
            var members = _context.TeamMembers.ToList();
            return View(members);
        }

        // GET: Create
        public IActionResult Create()
        {
            return View(new TeamMember());
        }
        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TeamMember member)
        {
            if (!ModelState.IsValid)
                return View(member); // لا تنشئ نموذج جديد، أعد نفس الـModel

            if (member.ImageFile != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/team");
                Directory.CreateDirectory(uploadsFolder);
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + member.ImageFile.FileName;
                using (var fileStream = new FileStream(Path.Combine(uploadsFolder, uniqueFileName), FileMode.Create))
                {
                    member.ImageFile.CopyTo(fileStream);
                }
                member.ImagePath = "/images/team/" + uniqueFileName;
            }

            _context.TeamMembers.Add(member);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // GET: Edit
        public IActionResult Edit(int id)
        {
            var member = _context.TeamMembers.Find(id);
            if (member == null) return NotFound();
            return View(member);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, TeamMember member)
        {
            var existingMember = _context.TeamMembers.Find(id);
            if (existingMember == null) return NotFound();

            if (ModelState.IsValid)
            {
                existingMember.FullName = member.FullName;
                existingMember.Role = member.Role;
                existingMember.Department = member.Department;
                existingMember.Bio = member.Bio;

                // تحديث الصورة إذا تم رفع صورة جديدة
                if (member.ImageFile != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/team");
                    Directory.CreateDirectory(uploadsFolder);
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + member.ImageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        member.ImageFile.CopyTo(fileStream);
                    }
                    existingMember.ImagePath = "/images/team/" + uniqueFileName;
                }

                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        // GET: Details
        public IActionResult Details(int id)
        {
            var member = _context.TeamMembers.Find(id);
            if (member == null) return NotFound();
            return View(member);
        }

        // GET: Delete
        public IActionResult Delete(int id)
        {
            var member = _context.TeamMembers.Find(id);
            if (member == null) return NotFound();

            _context.TeamMembers.Remove(member);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
