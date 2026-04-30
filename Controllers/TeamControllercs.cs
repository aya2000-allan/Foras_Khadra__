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
        public IActionResult Index(string search, string department, int page = 1)
        {
            int pageSize = 10;

            var members = _context.TeamMember.AsQueryable();

            // فلترة حسب الاسم
            if (!string.IsNullOrEmpty(search))
            {
                members = members.Where(m => m.NameAr.Contains(search)
                                           || m.NameEn.Contains(search)
                                           || m.NameFr.Contains(search));
            }

            // فلترة حسب القسم (Enum)
            if (!string.IsNullOrEmpty(department))
            {
                if (Enum.TryParse<Department>(department, out var deptEnum))
                {
                    members = members.Where(m => m.Department == deptEnum);
                }
            }

            // ترتيب ومجموع الصفحات
            members = members.OrderBy(m => m.Department)
                             .ThenByDescending(m => m.Membership);

            int totalMembers = members.Count();
            int totalPages = (int)Math.Ceiling(totalMembers / (double)pageSize);

            var pagedMembers = members.Skip((page - 1) * pageSize)
                                      .Take(pageSize)
                                      .ToList();

            // ارسال Enum للقسم للـ View
            ViewBag.Departments = Enum.GetValues(typeof(Department)).Cast<Department>().ToList();
            ViewBag.SelectedDepartment = department;

            ViewBag.Page = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.Search = search;

            return View(pagedMembers);
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
                NameAr = model.NameAr,
                NameEn = model.NameEn,
                NameFr = model.NameFr,

                BioAr = Regex.Replace(model.BioAr ?? "", "<.*?>", ""),
                BioEn = Regex.Replace(model.BioEn ?? "", "<.*?>", ""),
                BioFr = Regex.Replace(model.BioFr ?? "", "<.*?>", ""),
                Membership = model.Membership,
                Department = model.Department,
                Gender = model.Gender,
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
                NameAr = member.NameAr,
                NameEn = member.NameEn,
                NameFr = member.NameFr,

                BioAr = Regex.Replace(member.BioAr ?? "", "<.*?>", ""),
                BioEn = Regex.Replace(member.BioEn ?? "", "<.*?>", ""),
                BioFr = Regex.Replace(member.BioFr ?? "", "<.*?>", ""),
                Membership = member.Membership,
                Department = member.Department,
                Gender = member.Gender,
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

            member.NameAr = model.NameAr;
            member.NameEn = model.NameEn;
            member.NameFr = model.NameFr;

            member.BioAr = Regex.Replace(model.BioAr ?? "", "<.*?>", "");
            member.BioEn = Regex.Replace(model.BioEn ?? "", "<.*?>", "");
            member.BioFr = Regex.Replace(model.BioFr ?? "", "<.*?>", "");
            member.Membership = model.Membership;
            member.Department = model.Department;
            member.Gender = model.Gender;

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
