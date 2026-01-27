using Foras_Khadra.Data;
using Foras_Khadra.Models;
using Foras_Khadra.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
namespace Foras_Khadra.Controllers
{
    public class TeamController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public TeamController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // عرض كل الأعضاء
        public IActionResult Index()
        {
            var members = _context.TeamMember.ToList();
            return View(members);
        }

        // صفحة إنشاء عضو جديد
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TeamMemberCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var member = new TeamMember
            {
                Name = model.Name,
                Membership = model.Membership,
                Department = model.Department,
                Bio = Regex.Replace(model.Bio ?? "", "<.*?>", "")
            };

            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ImageFile.CopyTo(fileStream);
                }

                member.ImagePath = "/uploads/" + uniqueFileName;
            }

            _context.TeamMember.Add(member);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // GET: Edit
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var member = _context.TeamMember.Find(id);
            if (member == null) return NotFound();

            var model = new TeamMemberEditViewModel
            {
                Id = member.Id,
                Name = member.Name,
                Membership = member.Membership,
                Department = member.Department,
                Bio = member.Bio
            };

            ViewBag.ExistingImage = member.ImagePath;
            return View(model);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TeamMemberEditViewModel model)
        {
            var member = _context.TeamMember.Find(model.Id);
            if (member == null) return NotFound();

            // إزالة Validation على ImageFile لجعلها اختياري
            ModelState.Remove("ImageFile");

            if (!ModelState.IsValid)
            {
                ViewBag.ExistingImage = member.ImagePath;
                return View(model);
            }

            member.Name = model.Name;
            member.Membership = model.Membership;
            member.Department = model.Department;
            member.Bio = Regex.Replace(model.Bio ?? "", "<.*?>", "");

            // رفع صورة جديدة إذا تم اختيارها
            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ImageFile.CopyTo(fileStream);
                }

                // حذف الصورة القديمة إذا موجودة
                if (!string.IsNullOrEmpty(member.ImagePath))
                {
                    string oldPath = Path.Combine(_hostEnvironment.WebRootPath, member.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                member.ImagePath = "/uploads/" + uniqueFileName;
            }

            _context.Update(member);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        // تفاصيل العضو
        public IActionResult Details(int id)
        {
            var member = _context.TeamMember.Find(id);
            if (member == null) return NotFound();
            return View(member);
        }

        // حذف العضو
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var member = _context.TeamMember.Find(id);
            if (member != null)
            {
                // حذف الصورة من السيرفر إذا موجودة
                if (!string.IsNullOrEmpty(member.ImagePath))
                {
                    string fullPath = Path.Combine(_hostEnvironment.WebRootPath, member.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(fullPath))
                        System.IO.File.Delete(fullPath);
                }

                _context.TeamMember.Remove(member);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
