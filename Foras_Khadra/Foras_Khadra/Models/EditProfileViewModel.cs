using System.ComponentModel.DataAnnotations;

namespace Foras_Khadra.Models
{
    public class EditProfileViewModel
    {
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

        [MinLength(1, ErrorMessage = "اختر اهتمام واحد على الأقل")]
        public List<string> Interests { get; set; } = new();

        // للعرض
        public List<InterestItem> AvailableInterests { get; set; } = new()
{
    new InterestItem { Key = "competitions", DisplayName = "المسابقات" },
    new InterestItem { Key = "conferences", DisplayName = "المؤتمرات" },
    new InterestItem { Key = "volunteer_opportunities", DisplayName = "فرص التطوع" },
    new InterestItem { Key = "jobs", DisplayName = "الوظائف" },
    new InterestItem { Key = "grants", DisplayName = "المنح" },
    new InterestItem { Key = "fellowships", DisplayName = "الزمالات" },
    new InterestItem { Key = "training_opportunities", DisplayName = "فرص التدريب" }
};


        public List<string> Countries { get; set; } = new();
        public List<string> Nationalities { get; set; } = new();
    }
}
