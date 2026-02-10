using CountryData;
using Foras_Khadra.Data;
using Foras_Khadra.Models;
using Foras_Khadra.Resources.Views.Organization;
using Foras_Khadra.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Nager.Country;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        private readonly IStringLocalizer<OrganizationController> _localizer;

        public OrganizationController(
            ApplicationDbContext context,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender, IStringLocalizer<OrganizationController> localizer)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSender = emailSender;
            _localizer = localizer;
        }

        private List<SelectListItem> GetCountries()
        {
            // معرفة لغة الموقع الحالية: "ar", "en", "fr"
            var culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

            // نحصل على كل الثقافات المحددة بالدول
            var countries = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                .Select(c =>
                {
                    try
                    {
                        var region = new RegionInfo(c.Name);
                        string name = culture switch
                        {
                            "ar" => region.NativeName,   // الاسم المحلي (عادة عربي)
                            "fr" => region.EnglishName,  // الفرنسية ليس مباشرة، نضع EnglishName مؤقتاً
                            _ => region.EnglishName      // افتراضي إنجليزي
                        };
                        return new SelectListItem
                        {
                            Value = region.TwoLetterISORegionName, // الكود 2 حرف
                            Text = name
                        };
                    }
                    catch
                    {
                        return null;
                    }
                })
                .Where(x => x != null)
                .DistinctBy(x => x.Value) // تجنب التكرار
                .OrderBy(x => x.Text)
                .ToList();

            return countries;
        }

        // ====== REGISTER ======
        [HttpGet]
        public IActionResult Register()
        {
            var model = new RegisterOrganizationViewModel
            {
                Countries = GetCountries()
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

            // تحقق من أن الإيميل غير مستخدم
            var existingUser = await _userManager.FindByEmailAsync(model.ContactEmail);
            if (existingUser != null)
            {
                ModelState.AddModelError(nameof(model.ContactEmail),
                    RegisterOrganizationResources.EmailAlreadyUsed);

                // تمرير رقم الخطوة الحالية
                ViewData["CurrentStep"] = 4; // الخطوة الخاصة بالمعلومات الأساسية + البريد
                return View(model);
            }


            // تحقق من قوة كلمة المرور
            if (string.IsNullOrWhiteSpace(model.Password) ||
                model.Password.Length < 8 ||
                !model.Password.Any(char.IsUpper) ||
                !model.Password.Any(char.IsLower) ||
                !model.Password.Any(char.IsDigit) ||
                !model.Password.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                ModelState.AddModelError("Password", "كلمة المرور يجب أن تحتوي على 8 أحرف على الأقل، حرف كبير، حرف صغير، رقم ورمز خاص");
                return View(model);
            }

            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "كلمة المرور وتأكيدها غير متطابقين");
                return View(model);
            }

            // إنشاء Identity User
            var identityUser = new ApplicationUser
            {
                UserName = model.ContactEmail,
                Email = model.ContactEmail
            };

            var createResult = await _userManager.CreateAsync(identityUser, model.Password);
            if (!createResult.Succeeded)
            {
                foreach (var error in createResult.Errors)
                    ModelState.AddModelError("", error.Description);
                return View(model);
            }

            // إضافة الدور Organization
            if (!await _userManager.IsInRoleAsync(identityUser, "Organization"))
            {
                await _userManager.AddToRoleAsync(identityUser, "Organization");
            }

            // إنشاء Organization بدون لمس الباسورد (Password يُخزن hashed في IdentityUser)
            var organization = new Organization
            {
                Name = model.Name,
                Sector = model.Sector,
                Country = model.Country,
                Location = model.Location ?? "",
                ContactName = model.ContactName,
                ContactEmail = model.ContactEmail,
                PhoneNumber = model.PhoneNumber,
                Website = string.IsNullOrEmpty(model.Website) ? null : model.Website,
                CreatedAt = DateTime.Now,
                UserId = identityUser.Id,
                Password = new PasswordHasher<Organization>().HashPassword(null, model.Password) // ← مهم

            };

            _context.Organizations.Add(organization);
            await _context.SaveChangesAsync();

            return RedirectToAction("RegisterSuccess");
        }

        [HttpGet]
        public IActionResult RegisterSuccess() => View();

        // ====== LOGIN ======
        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(OrganizationLoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "البريد الإلكتروني أو كلمة المرور غير صحيحة");
                return View(model);
            }

            // التحقق من الدور
            var isOrganization = await _userManager.IsInRoleAsync(user, "Organization");
            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            var isNormalUser = await _userManager.IsInRoleAsync(user, "User"); // المستخدم العادي


            if (await _userManager.IsInRoleAsync(user, "User")) // المستخدم العادي
            {
                TempData["UserLoginAlert"] = _localizer["UserLoginAlert"].Value; // النص العربي/الإنجليزي/الفرنسي
                TempData["UserLoginRedirectText"] = _localizer["UserLoginRedirectText"].Value; // نص الزر
                TempData["UserLoginRedirect"] = Url.Action("Login", "Account"); // صفحة تسجيل دخول المستخدمين
                return RedirectToAction("Login");
            }

            // تحقق كلمة المرور
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

            // توجيه الادمن للوحة الإدارة والمنظمة للوحة التحكم الخاصة بها
            if (isAdmin)
                return RedirectToAction("Dashboard", "Admin");
            else
                return RedirectToAction("Index", "Home");
        }

        public IActionResult Dashboard() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // ====== PROFILE SETTING ======
        [HttpGet]
        public async Task<IActionResult> ProfileSetting()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return RedirectToAction("Login");

            var organization = await _context.Organizations
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.UserId == userId);

            if (organization == null)
                return NotFound();

            var model = new OrganizationProfileViewModel
            {
                Name = organization.Name ?? "",
                Sector = organization.Sector ?? "",
                Country = organization.Country ?? "",
                ContactEmail = organization.ContactEmail ?? "",
                PhoneNumber = organization.PhoneNumber ?? "",
                Location = organization.Location ?? "",
                ContactName = organization.ContactName ?? "",
                Website = organization.Website ?? "",
                Countries = GetCountries()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProfileSetting(OrganizationProfileViewModel model)
        {
            model.Countries = GetCountries();
            if (!ModelState.IsValid)
                return View(model);

            var userId = _userManager.GetUserId(User);
            var organization = await _context.Organizations
                .FirstOrDefaultAsync(o => o.UserId == userId);

            if (organization == null)
                return RedirectToAction("Login");

            var user = await _userManager.FindByIdAsync(organization.UserId);
            if (user == null)
                return RedirectToAction("Login");

            // ✨ تحديث الإيميل (الدخول + التواصل)
            if (!string.IsNullOrWhiteSpace(model.ContactEmail) &&
                model.ContactEmail != user.Email)
            {
                user.Email = model.ContactEmail;
                user.UserName = model.ContactEmail;
                user.NormalizedEmail = _userManager.NormalizeEmail(model.ContactEmail);
                user.NormalizedUserName = _userManager.NormalizeName(model.ContactEmail);

                await _userManager.UpdateAsync(user);
                organization.ContactEmail = model.ContactEmail;

                await _signInManager.RefreshSignInAsync(user);
            }

            // باقي البيانات
            organization.Name = model.Name ?? organization.Name;
            organization.Sector = model.Sector ?? organization.Sector;
            organization.Country = model.Country ?? organization.Country;
            organization.Location = model.Location ?? organization.Location;
            organization.ContactName = model.ContactName ?? organization.ContactName;
            organization.PhoneNumber = model.PhoneNumber ?? organization.PhoneNumber;
            organization.Website = string.IsNullOrWhiteSpace(model.Website) ? organization.Website : model.Website;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "تم تحديث البيانات بنجاح!";
            return RedirectToAction("ProfileSetting");
        }


        // ====== FORGOT / RESET PASSWORD ======
        [HttpGet]
        public IActionResult ForgotPassword() => View();

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
                return View();

            var organization = await _context.Organizations.FirstOrDefaultAsync(o => o.ContactEmail == email);
            if (organization == null)
                return View("ForgotPasswordConfirmation");

            organization.PasswordResetToken = Guid.NewGuid().ToString();
            organization.TokenExpiry = DateTime.Now.AddMinutes(30);
            await _context.SaveChangesAsync();

            var resetLink = Url.Action(
                "ResetPassword",
                "Organization",
                new { email = organization.ContactEmail, token = organization.PasswordResetToken },
                Request.Scheme
            );

            // رسالة الإيميل
            string emailBody = $@"
<!DOCTYPE html>
<html lang='{CultureInfo.CurrentCulture.TwoLetterISOLanguageName}' dir='{(CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "ar" ? "rtl" : "ltr")}'>
<head>
    <meta charset='UTF-8'>
    <title>{_localizer["ResetPasswordSubject"]}</title>
</head>
<body style='font-family: Tahoma, Arial; background-color: #f4f6f8; padding:20px;'>
    <div style='max-width:600px;margin:auto;background:#fff;padding:30px;border-radius:8px;'>
        <h2 style='text-align:center;'>Foras Khadra</h2>
        <p>{_localizer["ResetPasswordGreeting"]}</p>
        <p>{string.Format(_localizer["ResetPasswordBodyOrg"], organization.Name)}</p>
        <p style='text-align:center;margin:30px 0;'>
            <a href='{resetLink}' 
               style='background:#28a745; color:#fff; padding:12px 30px;
                      text-decoration:none; border-radius:6px; font-weight:bold;'>
                {_localizer["ResetPasswordButton"]}
            </a>
        </p>
        <p>{_localizer["ResetPasswordIgnore"]}</p>
        <p>{_localizer["ResetPasswordRegards"]}</p>
    </div>
</body>
</html>";


            string emailSubject = $"{_localizer["SiteName"]} - {_localizer["ResetPasswordSubject"]}";

            await _emailSender.SendEmailAsync(
    organization.ContactEmail,
    emailSubject,
    emailBody
); 
            return View("ForgotPasswordConfirmation");



        }

        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            var org = _context.Organizations.FirstOrDefault(o => o.ContactEmail == email);
            if (org == null || org.PasswordResetToken != token || org.TokenExpiry < DateTime.Now)
                return BadRequest("الرابط غير صالح أو منتهي الصلاحية.");

            return View(new OrganizationResetPasswordViewModel
            {
                Email = email,
                Token = token
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(OrganizationResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // احصل على المستخدم من Identity وليس من Organization
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (!await _userManager.IsInRoleAsync(user, "Organization"))
            {
                await _signInManager.SignOutAsync();
                ModelState.AddModelError("", "يجب تسجيل الدخول كمنظمة للوصول لهذه الصفحة.");
                return View(model);
            }

            var org = await _context.Organizations.FirstOrDefaultAsync(o => o.ContactEmail == model.Email);
            if (org == null || org.PasswordResetToken != model.Token || org.TokenExpiry < DateTime.Now)
                return BadRequest("الرابط غير صالح أو منتهي الصلاحية.");

            // إعادة تعيين كلمة المرور باستخدام UserManager
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetResult = await _userManager.ResetPasswordAsync(user, resetToken, model.Password);

            if (!resetResult.Succeeded)
            {
                foreach (var error in resetResult.Errors)
                    ModelState.AddModelError("", error.Description);
                return View(model);
            }

            // مسح التوكن من Organization بعد النجاح
            org.PasswordResetToken = null;
            org.TokenExpiry = null;
            await _context.SaveChangesAsync();

            return View("ResetPasswordConfirmation");
        }
    }
}
