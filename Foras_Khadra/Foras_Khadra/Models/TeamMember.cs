using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foras_Khadra.Models
{
    public class TeamMember
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "الاسم الكامل")]
        public string FullName { get; set; }

        [Display(Name = "الدور")]
        public Role Role { get; set; }  // مؤسس / مدير قسم / عضو عادي

        [Display(Name = "القسم")]
        public string Department { get; set; }

        [Display(Name = "السيرة الذاتية")]
        public string Bio { get; set; }

        [Display(Name = "الصورة")]
        public string ImagePath { get; set; }

        [NotMapped]
        [Display(Name = "رفع الصورة")]
        public IFormFile ImageFile { get; set; }
    }

    public enum Role
    {
        Founder,     // مؤسس
        Manager,     // مدير قسم
        Member       // عضو
    }
}