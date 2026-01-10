using Foras_Khadra.Models;
using Foras_Khadra.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Foras_Khadra.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Microsoft.AspNetCore.Identity.UI.Services.IEmailSender _emailSender;

        // بيانات Admin الثابت
        private const string AdminEmail = "admin@org.com";
        private const string AdminPassword = "Admin@123";

        public AccountController(
            Microsoft.AspNetCore.Identity.UI.Services.IEmailSender emailSender,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        // ===== GET: RegisterUser =====
        [HttpGet]
        public IActionResult RegisterUser()
        {
            var model = new RegisterViewModel
            {
                Countries = GetCountries(),
                Nationalities = GetCountries(),
                AvailableInterests = new List<string> { "المسابقات", "المؤتمرات", "فرص التطوع", "الوظائف", "المنح", "الزمالات", "فرص التدريب" }
            };
            return View(model);
        }

        // ===== POST: RegisterUser =====
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterUser(RegisterViewModel model)
        {
            model.Countries = GetCountries();
            model.Nationalities = GetCountries();
            model.AvailableInterests = new List<string> { "المسابقات", "المؤتمرات", "فرص التطوع", "الوظائف", "المنح", "الزمالات", "فرص التدريب" };

            if (!ModelState.IsValid) return View(model);

            if (model.Interests == null || !model.Interests.Any())
            {
                ModelState.AddModelError("Interests", "اختر اهتمام واحد على الأقل");
                return View(model);
            }

            // ===== تحقق من أن الإيميل غير مستخدم =====
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "هذا البريد الإلكتروني مستخدم من قبل");
                return View(model); // الفورم يرجع مع البيانات محفوظة
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                FullName = $"{model.FirstName} {model.LastName}".Trim(),
                Country = model.Country,
                Nationality = model.Nationality,
                Interests = model.Interests,
                Role = UserRole.User,
                CreatedAt = System.DateTime.Now
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
                return View(model);
            }

            // إضافة الدور User (تأكد من أنه موجود)
            await _userManager.AddToRoleAsync(user, "User");

            return RedirectToAction("RegisterConfirmation");
        }

        [HttpGet]
        public IActionResult RegisterConfirmation() => View();

        // ===== GET: Login =====
        [HttpGet]
        public IActionResult Login() => View(new LoginViewModel());

        // ===== POST: Login =====
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "البريد الإلكتروني أو كلمة المرور غير صحيحة");
                return View(model);
            }

            // تحقق كلمة المرور باستخدام SignInManager
            var result = await _signInManager.PasswordSignInAsync(
                user,
                model.Password,
                isPersistent: false,
                lockoutOnFailure: false
            );

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "البريد الإلكتروني أو كلمة المرور غير صحيحة");
                return View(model);
            }

            // تحقق الدور لتوجيه المستخدم
            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return RedirectToAction("Dashboard", "Admin");
            }
            else
            {
                return RedirectToAction("Dashboard", "User");
            }
        }

        // ===== Logout =====
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // ===== مساعد لجلب الدول والجنسية =====
        private List<string> GetCountries()
        {
            return CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                .Select(c => {
                    try { return new RegionInfo(c.Name).EnglishName; }
                    catch { return null; }
                })
                .Where(r => !string.IsNullOrEmpty(r))
                .Distinct()
                .OrderBy(r => r)
                .ToList();
        }
    }
}