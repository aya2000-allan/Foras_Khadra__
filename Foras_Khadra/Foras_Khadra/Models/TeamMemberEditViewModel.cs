using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Foras_Khadra.Models;

namespace Foras_Khadra.ViewModels
{
    public class TeamMemberEditViewModel
    {
        public int Id { get; set; }

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

        [Display(Name = "رفع صورة جديدة (اختياري)")]
        public IFormFile ImageFile { get; set; }
    }
}
