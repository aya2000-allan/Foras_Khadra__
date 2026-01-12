using System.ComponentModel.DataAnnotations;
using Foras_Khadra.Models;

namespace Foras_Khadra.ViewModels
{
    public class TeamMemberCreateViewModel
    {
        [Required]
        [Display(Name = "الاسم")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "نوع العضوية")]
        public MembershipType Membership { get; set; }

        [Required]
        [Display(Name = "القسم")]
        public Department Department { get; set; }

        [Display(Name = "السيرة الذاتية")]
        public string Bio { get; set; }

        [Display(Name = "صورة العضو")]
        public IFormFile ImageFile { get; set; } // لرفع الصورة فقط
    }
}
