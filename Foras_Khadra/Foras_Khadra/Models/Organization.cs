using System;
using System.ComponentModel.DataAnnotations;

namespace Foras_Khadra.Models
{
    public class Organization
    {
        public int Id { get; set; }


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

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "كلمة المرور يجب أن تكون على الأقل 8 أحرف")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$",
    ErrorMessage = "كلمة المرور يجب أن تحتوي على حرف كبير، حرف صغير، رقم ورمز خاص")]
        public string Password { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Location { get; internal set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? TokenExpiry { get; set; }

    }
}
