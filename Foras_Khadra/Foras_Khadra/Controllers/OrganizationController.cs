using CountryData; // مكتبة CountryData
using Foras_Khadra.Data;
using Foras_Khadra.Models;
using Foras_Khadra.Migrations;
using Foras_Khadra.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nager.Country;
using Microsoft.AspNetCore.Identity; // << مهم لـ PasswordHasher
using Microsoft.AspNetCore.Http; // << مهم لـ Session

namespace Foras_Khadra.Controllers
{
    public class OrganizationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrganizationController(
            ApplicationDbContext context,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }


        private List<SelectListItem> GetCountries()
        {
            var provider = new CountryProvider();
            var countries = provider.GetCountries();

            return countries
                .OrderBy(c => c.CommonName)
                .Select(c => new SelectListItem
                {
                    Value = c.CommonName,
                    Text = c.CommonName
                })
                .ToList();
        }



        [HttpGet]
        public IActionResult Register()
        {
            var model = new RegisterOrganizationViewModel
            {
                Countries = GetCountries(),
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterOrganizationViewModel model)
        {
            model.Countries = GetCountries();

            if (!ModelState.IsValid)
                return View(model);

            // ===== تحقق من أن الإيميل غير مستخدم =====
            var existingOrg = _context.Organizations
                .FirstOrDefault(o => o.ContactEmail.ToLower() == model.ContactEmail.ToLower());

            if (existingOrg != null)
            {
                ModelState.AddModelError(nameof(model.ContactEmail),
                    "هذا البريد الإلكتروني مستخدم من قبل");

                // مهم جدًا: إعادة تعبئة القوائم
                model.Countries = GetCountries();

                return View(model);
            }

            // ===== تحقق من قوة كلمة المرور =====
            if (string.IsNullOrWhiteSpace(model.Password) ||
                model.Password.Length < 8 ||
                !model.Password.Any(char.IsUpper) ||
                !model.Password.Any(char.IsLower) ||
                !model.Password.Any(char.IsDigit) ||
                !model.Password.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                ModelState.AddModelError("Password", "كلمة المرور يجب أن تحتوي على 8 أحرف على الأقل، حرف كبير، حرف صغير، رقم ورمز خاص");
            }

            // ===== تحقق من ConfirmPassword =====
            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "كلمة المرور وتأكيدها غير متطابقين");
            }

            if (!ModelState.IsValid)
            {
                return View(model); // ⚠️ الفورم يرجع مع حفظ باقي البيانات
            }

            var organization = new Organization
            {
                Name = model.Name,
                Sector = model.Sector,
                Country = model.Country,
                Location = model.Location,
                ContactName = model.ContactName,
                ContactEmail = model.ContactEmail,
                PhoneNumber = model.PhoneNumber,
                Website = string.IsNullOrEmpty(model.Website) ? null : model.Website,
                CreatedAt = DateTime.Now
            };

            // Hash كلمة المرور
            var hasher = new PasswordHasher<Organization>();
            organization.Password = hasher.HashPassword(organization, model.Password);

            _context.Organizations.Add(organization);
            await _context.SaveChangesAsync();

            return RedirectToAction("RegisterSuccess");
        }


        [HttpGet]
        public IActionResult RegisterSuccess()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(OrganizationLoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var organization = _context.Organizations
                .FirstOrDefault(o => o.ContactEmail == model.Email);

            if (organization != null)
            {
                var hasher = new PasswordHasher<Organization>();
                var result = hasher.VerifyHashedPassword(organization, organization.Password, model.Password);

                if (result == PasswordVerificationResult.Success)
                {
                    // إنشاء Identity User مؤقت للمنظمة
                    var appUser = new ApplicationUser
                    {
                        UserName = organization.ContactEmail,
                        Email = organization.ContactEmail
                    };

                    // تسجيل الدخول باستخدام SignInManager
                    await _signInManager.SignInAsync(appUser, isPersistent: false);

                    // إضافة الدور "Organization"
                    // ملاحظة: إذا كنت تستخدم Roles، تأكد أن الدور موجود في قاعدة البيانات
                    // await _userManager.AddToRoleAsync(appUser, "Organization");

                    return RedirectToAction("Dashboard", "Organization");
                }
            }

            ModelState.AddModelError("", "البريد الإلكتروني أو كلمة المرور غير صحيحة");
            return View(model);
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        // ===== Logout =====
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}
