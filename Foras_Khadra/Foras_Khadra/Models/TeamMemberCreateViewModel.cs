using System.ComponentModel.DataAnnotations;
using Foras_Khadra.Models;

namespace Foras_Khadra.ViewModels
{
    public class TeamMemberCreateViewModel
    {
        [Required]
        [Display(Name = "الاسم بالعربي")]
        public string NameAr { get; set; }

        [Display(Name = "الاسم بالانجليزي")]
        public string NameEn { get; set; }

        [Display(Name = "الاسم بالفرنسي")]
        public string NameFr { get; set; }

        [Required]
        [Display(Name = "نوع العضوية")]
        public MembershipType Membership { get; set; }

        [Required]
        [Display(Name = "القسم")]
        public Department Department { get; set; }

        [Display(Name = "البيو بالعربي")]
        public string BioAr { get; set; }

        [Display(Name = "البيو بالانجليزي")]
        public string BioEn { get; set; }

        [Display(Name = "البيو بالفرنسي")]
        public string BioFr { get; set; }

        [Display(Name = "صورة العضو")]
        public IFormFile ImageFile { get; set; } // لرفع الصورة فقط

        [Required]
        [Display(Name = "جنس العضو")]

        public GenderType Gender { get; set; }
    }
}
