using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Foras_Khadra.ViewModels
{
    public class RegisterOrganizationViewModel
    {
        // Tab 1: Account Type
        [Required(ErrorMessage = "اختر نوع الحساب")]
        public string AccountType { get; set; } // "فردي" أو "منظمة"

        // Tab 2: Organization Info
        [Required(ErrorMessage = "اسم المنظمة مطلوب")]
        public string Name { get; set; }

        [Required(ErrorMessage = "القطاع مطلوب")]
        public string Sector { get; set; }

        // Tab 3: Country & Location
        [Required(ErrorMessage = "اختر الدولة")]
        public string Country { get; set; }

        [Required(ErrorMessage = "الموقع مطلوب")]
        public string Location { get; set; }

        // Tab 4: Personal Info
        [Required(ErrorMessage = "اسم جهة الاتصال مطلوب")]
        public string ContactName { get; set; }

        [Required(ErrorMessage = "البريد الرسمي مطلوب")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
        public string ContactEmail { get; set; }

        public string? Website { get; set; } // optional

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "تأكيد كلمة المرور مطلوب")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "كلمتا المرور غير متطابقتين")]
        public string ConfirmPassword { get; set; }

        // القوائم للاستدعاء في الفيو
        public List<SelectListItem> Countries { get; set; } = new List<SelectListItem>();
    }
}
