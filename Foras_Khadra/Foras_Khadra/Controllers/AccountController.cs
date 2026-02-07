using Foras_Khadra.Models;
using Foras_Khadra.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using NETCore.MailKit.Core;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Foras_Khadra.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Microsoft.AspNetCore.Identity.UI.Services.IEmailSender _emailSender;
        private readonly IStringLocalizer<AccountController> _localizer;

        // بيانات Admin الثابت
        private const string AdminEmail = "admin@org.com";
        private const string AdminPassword = "Admin@123";

        public AccountController(
            Microsoft.AspNetCore.Identity.UI.Services.IEmailSender emailSender,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager, IStringLocalizer<AccountController> localizer)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSender = emailSender;
            _localizer = localizer;
        }

        // ===== GET: RegisterUser =====
        [HttpGet]
        public IActionResult RegisterUser()
        {
            var model = new RegisterViewModel
            {
                Countries = GetCountries(),
                Nationalities = GetCountries(),
                AvailableInterests = new List<InterestItem>
        {
            new InterestItem { Key = "competitions", DisplayName = "المسابقات" },
            new InterestItem { Key = "conferences", DisplayName = "المؤتمرات" },
            new InterestItem { Key = "volunteer_opportunities", DisplayName = "فرص التطوع" },
            new InterestItem { Key = "jobs", DisplayName = "الوظائف" },
            new InterestItem { Key = "grants", DisplayName = "المنح" },
            new InterestItem { Key = "fellowships", DisplayName = "الزمالات" },
            new InterestItem { Key = "training_opportunities", DisplayName = "فرص التدريب" }
        }
            };
            return View(model);
        }

        // ===== POST: RegisterUser =====
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterUser(RegisterViewModel model)
        {
            var culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            model.Countries = GetCountries(culture);
            model.Nationalities = GetCountries(culture);
            model.AvailableInterests = new List<InterestItem>
    {
        new InterestItem { Key = "competitions", DisplayName = "المسابقات" },
        new InterestItem { Key = "conferences", DisplayName = "المؤتمرات" },
        new InterestItem { Key = "volunteer_opportunities", DisplayName = "فرص التطوع" },
        new InterestItem { Key = "jobs", DisplayName = "الوظائف" },
        new InterestItem { Key = "grants", DisplayName = "المنح" },
        new InterestItem { Key = "fellowships", DisplayName = "الزمالات" },
        new InterestItem { Key = "training_opportunities", DisplayName = "فرص التدريب" }
    };

            if (!ModelState.IsValid) return View(model);

            if (model.Interests == null || !model.Interests.Any())
            {
                ModelState.AddModelError("Interests", "اختر اهتمام واحد على الأقل");
                return View(model);
            }

            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "هذا البريد الإلكتروني مستخدم من قبل");
                return View(model);
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
                Interests = model.Interests, // هنا نخزن الـ Key فقط
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

            if (await _userManager.IsInRoleAsync(user, "Organization"))
            {
                TempData["OrgLoginAlert"] = _localizer["OrgLoginAlert"].Value; // نص عربي/إنجليزي جاهز
                TempData["OrgLoginRedirectText"] = _localizer["OrgLoginRedirectText"].Value;
                TempData["OrgLoginRedirect"] = Url.Action("Login", "Organization");
                return RedirectToAction("Login");
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
                return RedirectToAction("Index", "Home");
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
        private List<string> GetCountries(string culture = null)
        {
            culture ??= CultureInfo.CurrentUICulture.TwoLetterISOLanguageName; // ar / en
            return CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                .Select(c =>
                {
                    try
                    {
                        var region = new RegionInfo(c.Name);
                        return culture == "ar" ? region.NativeName : region.EnglishName;
                    }
                    catch { return null; }
                })
                .Where(r => !string.IsNullOrEmpty(r))
                .Distinct()
                .OrderBy(r => r)
                .ToList();
        }

        // ==== Forgot Password ====
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
                return View();

            //  نجيب المستخدم
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return View("ForgotPasswordConfirmation");
            // مهم: ما نفضح إذا الإيميل موجود أو لا

            //  توليد Token
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            //  توليد رابط إعادة التعيين
            var resetLink = Url.Action(
                "ResetPassword",
                "Account",
                new { email = user.Email, token = token },
                Request.Scheme
            );

            string emailBody = $@"
<!DOCTYPE html>
<html lang='{CultureInfo.CurrentCulture.TwoLetterISOLanguageName}' dir='{(CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "ar" ? "rtl" : "ltr")}'>
<head>
    <meta charset='UTF-8'>
    <title>{_localizer["ResetPasswordSubject"]}</title>
</head>
<body style='font-family: Tahoma, Arial, sans-serif; background-color: #f4f6f8; padding: 20px;'>
    <div style='max-width: 600px; margin: auto; background-color: #ffffff; padding: 30px; border-radius: 8px;'>

        <h2 style='text-align: center; color: #2c3e50;'>Foras Khadra</h2>

        <p>{_localizer["ResetPasswordGreeting"]}</p>

        <p>{_localizer["ResetPasswordBody"]}</p>

        <div style='text-align: center; margin: 30px 0;'>
            <a href='{resetLink}'
               style='background-color: #28a745; color: #ffffff; padding: 12px 30px;
                      text-decoration: none; border-radius: 6px; font-size: 16px; font-weight: bold;'>
                {_localizer["ResetPasswordButton"]}
            </a>
        </div>

        <p>{_localizer["ResetPasswordIgnore"]}</p>

        <p>{_localizer["ResetPasswordRegards"]}</p>

        <hr style='margin-top: 30px;' />

        <p style='font-size: 12px; color: #777; text-align: center;'>
            {_localizer["ResetPasswordFooter"]}
        </p>
    </div>
</body>
</html>
";

            //  إرسال الإيميل
            string emailSubject = $"{_localizer["SiteName"]} - {_localizer["ResetPasswordSubject"]}";

            await _emailSender.SendEmailAsync(
                user.Email,
                emailSubject,
                emailBody
            );

            //  صفحة تأكيد الإرسال
            return View("ForgotPasswordConfirmation");
        }


        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
                return RedirectToAction("ForgotPassword");

            var model = new ResetPasswordViewModel
            {
                Email = email,
                Token = token
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return RedirectToAction("ResetPasswordConfirmation"); // لا نكشف إذا الإيميل موجود

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                // كلمة المرور نجحت → Redirect للـ Confirmation
                return RedirectToAction("ResetPasswordConfirmation");
            }

            // إذا فشل، اعرض كل الأخطاء (Token منتهي أو PasswordPolicy)
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ProfileSettings()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login");

            var model = new EditProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Country = user.Country,
                Nationality = user.Nationality,
                Interests = user.Interests, // قائمة الـ Keys المختارة
                Countries = GetCountries(),
                Nationalities = GetCountries(),
                AvailableInterests = new List<InterestItem> // لازم نضيفها
        {
            new InterestItem { Key = "competitions", DisplayName = "المسابقات" },
            new InterestItem { Key = "conferences", DisplayName = "المؤتمرات" },
            new InterestItem { Key = "volunteer_opportunities", DisplayName = "فرص التطوع" },
            new InterestItem { Key = "jobs", DisplayName = "الوظائف" },
            new InterestItem { Key = "grants", DisplayName = "المنح" },
            new InterestItem { Key = "fellowships", DisplayName = "الزمالات" },
            new InterestItem { Key = "training_opportunities", DisplayName = "فرص التدريب" }
        }
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProfileSettings(EditProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
                model.Countries = GetCountries(culture);
                model.Nationalities = GetCountries(culture);
                model.AvailableInterests = new List<InterestItem> // لازم نضيفها
        {
            new InterestItem { Key = "competitions", DisplayName = "المسابقات" },
            new InterestItem { Key = "conferences", DisplayName = "المؤتمرات" },
            new InterestItem { Key = "volunteer_opportunities", DisplayName = "فرص التطوع" },
            new InterestItem { Key = "jobs", DisplayName = "الوظائف" },
            new InterestItem { Key = "grants", DisplayName = "المنح" },
            new InterestItem { Key = "fellowships", DisplayName = "الزمالات" },
            new InterestItem { Key = "training_opportunities", DisplayName = "فرص التدريب" }
        };
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login");

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.FullName = $"{model.FirstName} {model.LastName}";
            user.Country = model.Country;
            user.Nationality = model.Nationality;
            user.Interests = model.Interests; // حفظ الـ Keys

            if (user.Email != model.Email)
            {
                user.Email = model.Email;
                user.UserName = model.Email;

                user.NormalizedEmail = _userManager.NormalizeEmail(model.Email);
                user.NormalizedUserName = _userManager.NormalizeName(model.Email);
            }

            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);

            TempData["Success"] = "تم تحديث بياناتك بنجاح";
            return RedirectToAction("ProfileSettings");
        }


    }
}