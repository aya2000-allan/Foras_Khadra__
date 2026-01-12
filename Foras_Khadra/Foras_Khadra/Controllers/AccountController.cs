using Foras_Khadra.Models;
using Foras_Khadra.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using NETCore.MailKit.Core;

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
                .Select(c =>
                {
                    try { return new RegionInfo(c.Name).EnglishName; }
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

            // 1️⃣ نجيب المستخدم
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return View("ForgotPasswordConfirmation");
            // مهم: ما نفضح إذا الإيميل موجود أو لا

            // 2️⃣ توليد Token
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // 3️⃣ توليد رابط إعادة التعيين
            var resetLink = Url.Action(
                "ResetPassword",
                "Account",
                new { email = user.Email, token = token },
                Request.Scheme
            );

            string emailBody = $@"
<!DOCTYPE html>
<html lang='ar' dir='rtl'>
<head>
    <meta charset='UTF-8'>
    <title>إعادة تعيين كلمة المرور</title>
</head>
<body style='font-family: Tahoma, Arial, sans-serif; background-color: #f4f6f8; padding: 20px;'>
    <div style='max-width: 600px; margin: auto; background-color: #ffffff; padding: 30px; border-radius: 8px;'>
        
        <h2 style='text-align: center; color: #2c3e50;'>Foras Khadra</h2>

        <p>مرحبًا،</p>

        <p>
            لقد تلقّينا طلبًا لإعادة تعيين كلمة المرور الخاصة بحسابك على منصة
            <strong>Foras Khadra</strong>.
        </p>

        <p>
            لإعادة تعيين كلمة المرور، يرجى الضغط على الزر أدناه:
        </p>

        <div style='text-align: center; margin: 30px 0;'>
            <a href='{resetLink}'
               style='background-color: #28a745; color: #ffffff; padding: 12px 30px;
                      text-decoration: none; border-radius: 6px; font-size: 16px; font-weight: bold;'>
                إعادة تعيين كلمة المرور
            </a>
        </div>

        <p>
            إذا لم تقم بطلب إعادة تعيين كلمة المرور، يرجى تجاهل هذا البريد الإلكتروني.
        </p>

        <p>
            مع تحيات فريق <strong>Foras Khadra</strong>
        </p>

        <hr style='margin-top: 30px;' />

        <p style='font-size: 12px; color: #777; text-align: center;'>
            هذا البريد الإلكتروني مرسل تلقائيًا، الرجاء عدم الرد عليه.
        </p>
    </div>
</body>
</html>
";

            // 5️⃣ إرسال الإيميل
            await _emailSender.SendEmailAsync(
                user.Email,
                "إعادة تعيين كلمة المرور - Foras Khadra",
                emailBody
            );

            // 6️⃣ صفحة تأكيد الإرسال
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
                Interests = user.Interests,
                Countries = GetCountries(),
                Nationalities = GetCountries()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProfileSettings(EditProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Countries = GetCountries();
                model.Nationalities = GetCountries();
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login");

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.FullName = $"{model.FirstName} {model.LastName}";
            user.Country = model.Country;
            user.Nationality = model.Nationality;
            user.Interests = model.Interests;

            //  تعديل الإيميل بدون التأثير على الباسورد
            if (user.Email != model.Email)
            {
                user.Email = model.Email;
                user.UserName = model.Email;

                user.NormalizedEmail = _userManager.NormalizeEmail(model.Email);
                user.NormalizedUserName = _userManager.NormalizeName(model.Email);
            }

            await _userManager.UpdateAsync(user);

            // (اختياري لكن مفضل)
            await _signInManager.RefreshSignInAsync(user);

            TempData["Success"] = "تم تحديث بياناتك بنجاح";
            return RedirectToAction("ProfileSettings");
        }


    }
}