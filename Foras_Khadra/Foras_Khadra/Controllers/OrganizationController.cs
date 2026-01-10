using CountryData; // مكتبة CountryData
using Foras_Khadra.Data;
using Foras_Khadra.Migrations;
using Foras_Khadra.Models;
using Foras_Khadra.ViewModels;
using Microsoft.AspNetCore.Http; // << مهم لـ Session
using Microsoft.AspNetCore.Identity; // << مهم لـ PasswordHasher
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nager.Country;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foras_Khadra.Controllers
{
    public class OrganizationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;


        public OrganizationController(
            ApplicationDbContext context,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSender = emailSender;
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

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
                return View();

            var organization = _context.Organizations.FirstOrDefault(o => o.ContactEmail == email);

            if (organization == null)
                return View("ForgotPasswordConfirmation");

            // توليد توكين عشوائي
            organization.PasswordResetToken = Guid.NewGuid().ToString();
            organization.TokenExpiry = DateTime.Now.AddMinutes(30); // صلاحية 30 دقيقة

            await _context.SaveChangesAsync();

            // إنشاء رابط إعادة تعيين
            var resetLink = Url.Action(
                "ResetPassword",
                "Organization",
                new { email = organization.ContactEmail, token = organization.PasswordResetToken },
                Request.Scheme
            );

            // رسالة الإيميل
            string emailBody = $@"
<!DOCTYPE html>
<html lang='ar' dir='rtl'>
<head>
    <meta charset='UTF-8'>
    <title>إعادة تعيين كلمة المرور</title>
</head>
<body style='font-family: Tahoma, Arial; background-color: #f4f6f8; padding:20px;'>
    <div style='max-width:600px;margin:auto;background:#fff;padding:30px;border-radius:8px;'>
        <h2 style='text-align:center;'>Foras Khadra</h2>
        <p>تحية طيبة،</p>
        <p>تم استلام طلب لإعادة تعيين كلمة المرور الخاصة بحساب منظمتكم <strong>{organization.Name}</strong> على منصة Foras Khadra.</p>
        <p style='text-align:center;margin:30px 0;'>
            <a href='{resetLink}' 
               style='background:#28a745; /* اللون الأخضر */
                      color:#fff;
                      padding:12px 30px;
                      text-decoration:none;
                      border-radius:6px;
                      font-weight:bold;'>
                إعادة تعيين كلمة المرور
            </a>
        </p>
        <p>إذا لم تقم منظمتكم بطلب إعادة تعيين كلمة المرور، يرجى تجاهل هذا البريد الإلكتروني.</p>
        <p>مع فائق الاحترام،<br/>فريق <strong>Foras Khadra</strong></p>
    </div>
</body>
</html>";

            await _emailSender.SendEmailAsync(
                organization.ContactEmail,
                "إعادة تعيين كلمة المرور – Foras Khadra",
                emailBody
            );

            return View("ForgotPasswordConfirmation");
        }

        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            var organization = _context.Organizations.FirstOrDefault(o => o.ContactEmail == email);

            if (organization == null || organization.PasswordResetToken != token || organization.TokenExpiry < DateTime.Now)
                return BadRequest("الرابط غير صالح أو منتهي الصلاحية.");

            var model = new OrganizationResetPasswordViewModel
            {
                Email = email,
                Token = token
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(OrganizationResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var organization = _context.Organizations.FirstOrDefault(o => o.ContactEmail == model.Email);

            if (organization == null || organization.PasswordResetToken != model.Token || organization.TokenExpiry < DateTime.Now)
                return BadRequest("الرابط غير صالح أو منتهي الصلاحية.");

            // Hash الباسورد الجديد
            var hasher = new PasswordHasher<Organization>();
            organization.Password = hasher.HashPassword(organization, model.Password);

            // مسح التوكين بعد الاستخدام
            organization.PasswordResetToken = null;
            organization.TokenExpiry = null;

            await _context.SaveChangesAsync();

            return View("ResetPasswordConfirmation");
        }


        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

    }
}
