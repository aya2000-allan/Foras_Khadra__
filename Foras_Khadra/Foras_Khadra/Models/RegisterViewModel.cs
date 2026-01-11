using System.ComponentModel.DataAnnotations;

namespace Foras_Khadra.ViewModels
{
    public class RegisterViewModel
    {
        // ===== للفرد =====
        [Required(ErrorMessage = "الاسم الأول مطلوب")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "الاسم الأخير مطلوب")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صالح")]
        public string Email { get; set; }

        [Required(ErrorMessage = "الدولة مطلوبة")]
        public string Country { get; set; }

        [Required(ErrorMessage = "الجنسية مطلوبة")]
        public string Nationality { get; set; }

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "كلمة المرور يجب أن تكون على الأقل 8 أحرف")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$",
    ErrorMessage = "كلمة المرور يجب أن تحتوي على حرف كبير، حرف صغير، رقم ورمز خاص")]
        public string Password { get; set; }

        [Required(ErrorMessage = "تأكيد كلمة المرور مطلوب")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "كلمة المرور غير متطابقة")]
        public string ConfirmPassword { get; set; }

        [MinLength(1, ErrorMessage = "اختر اهتمام واحد على الأقل")]
        public List<string> Interests { get; set; } = new List<string>(); // ما يلمس asp-for مباشرة
        public List<string> AvailableInterests { get; set; } = new List<string> // الخيارات
    {
        "المسابقات", "المؤتمرات", "فرص التطوع", "الوظائف", "المنح", "الزمالات", "فرص التدريب"
    };
        public List<string> Countries { get; set; } = new List<string>();
        public List<string> Nationalities { get; set; } = new List<string>();
    }
}