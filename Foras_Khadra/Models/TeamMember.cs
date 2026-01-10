using System.ComponentModel.DataAnnotations;

namespace Foras_Khadra.Models
{
    public class TeamMember
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

        [Display(Name = "صورة العضو")]
        public string ImagePath { get; set; }
    }

    public enum MembershipType
    {
        Member,
        Founder,
        Manager
    }

    public enum Department
    {
        Technology,
        HR,
        SocialMedia
    }
}
