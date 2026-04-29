using System.ComponentModel.DataAnnotations;

namespace Foras_Khadra.Models
{
    public class TeamMember
    {
        public int Id { get; set; }

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
        public string ImagePath { get; set; }

        [Display(Name = "جنس العضو")]

        public GenderType Gender { get; set; }

        [Display(Name = "تاريخ الإنضمام")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;


    }

    public enum GenderType
    {
        Male,
        Female
    }
    public enum MembershipType
    {
        Member,
        Founder,
        Manager,
        Coordinator,
        Officer,
        Deputy_Director,
        Avisor,

    }

    public enum Department
    {
        Technology,
        HR,
        SocialMedia,
        BoardOfDirectors,
        HonoraryMembers,
        Opportunities,
        PublicRelations,
        LocalTeams,
        Operations,


    }
}
