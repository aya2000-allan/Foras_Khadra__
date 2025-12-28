using Foras_Khadra.Models;
using Foras_Khadra.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        [HttpPost]
        public async Task<IActionResult> Index(Contact contact)
        {
            // تجاهل ModelState في حالة fetch
            if (!string.IsNullOrEmpty(contact.Honeypot))
                return BadRequest("Bot detected");

            // تحقق reCAPTCHA كما كان
            var recaptchaToken = Request.Form["g-recaptcha-response"].FirstOrDefault();
            if (string.IsNullOrEmpty(recaptchaToken))
                return BadRequest("reCAPTCHA token is missing.");

            // تحقق رقم الهاتف كما في الكود السابق
            var phoneUtil = PhoneNumberUtil.GetInstance();
            try
            {
                var phoneNumber = phoneUtil.Parse(contact.Phone, null);
                if (!phoneUtil.IsValidNumber(phoneNumber))
                    return BadRequest("رقم الهاتف غير صالح");

                contact.Phone = phoneUtil.Format(phoneNumber, PhoneNumberFormat.E164);
            }
            catch (NumberParseException)
            {
                return BadRequest("كود الدولة أو رقم الهاتف غير صحيح");
            }

            contact.SubmissionDateTime = DateTime.UtcNow;
            await _mailService.SendEmailAsync(contact);

            // ✅ أرسل رسالة نجاح مباشرة
            return Ok("تم إرسال الرسالة بنجاح!");
        }
           
        }
    }

