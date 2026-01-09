using System;
using System.ComponentModel.DataAnnotations;

namespace Foras_Khadra.Models
{
    public class Organization
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اختر نوع الحساب")]
        public string AccountType { get; set; } // "منظمة" أو "فردي"

        [Required(ErrorMessage = "ادخل اسم المنظمة")]
        public string Name { get; set; }

        [Required(ErrorMessage = "ادخل القطاع")]
        public string Sector { get; set; }

        [Required(ErrorMessage = "اختر الدولة")]
        public string Country { get; set; }


        [Required(ErrorMessage = "ادخل اسم جهة الاتصال")]
        public string ContactName { get; set; }

        [Required(ErrorMessage = "ادخل البريد الإلكتروني")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صالح")]
        public string ContactEmail { get; set; }

        public string? Website { get; set; } // ← اختياري

        [Required(ErrorMessage = "ادخل رقم الهاتف")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "ادخل كلمة المرور")]
        public string Password { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Location { get; internal set; }
    }
}
