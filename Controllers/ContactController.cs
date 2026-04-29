using Foras_Khadra.Models;
using Foras_Khadra.Services;
using Microsoft.AspNetCore.Mvc;
using PhoneNumbers;

namespace Foras_Khadra.Controllers
{
    public class ContactController : Controller
    {
        private readonly IMailService _mailService;
        public ContactController(IMailService mailService)
        {
            _mailService = mailService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Contact contact)
        {
            if (!string.IsNullOrEmpty(contact.Honeypot))
                return Json(new { success = false, message = "Bot detected" });

            // var recaptchaToken = Request.Form["g-recaptcha-response"].FirstOrDefault();
            // if (string.IsNullOrEmpty(recaptchaToken))
            //     return Json(new { success = false, message = "reCAPTCHA token is missing." });

            var phoneUtil = PhoneNumberUtil.GetInstance();
            try
            {
                var phoneNumber = phoneUtil.Parse(contact.Phone, null);
                if (!phoneUtil.IsValidNumber(phoneNumber))
                    return Json(new { success = false, message = "رقم الهاتف غير صالح" });

                contact.Phone = phoneUtil.Format(phoneNumber, PhoneNumberFormat.E164);
            }
            catch
            {
                return Json(new { success = false, message = "كود الدولة أو رقم الهاتف غير صحيح" });
            }

            contact.SubmissionDateTime = DateTime.UtcNow;

            try
            {
                await _mailService.SendEmailAsync(contact);
                return Json(new { success = true, message = "تم إرسال الرسالة بنجاح!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"خطأ في الإرسال: {ex.Message}" });
            }
        }
    }
}
