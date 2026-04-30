using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Foras_Khadra.Models;

namespace Foras_Khadra.ViewModels
{
    public class TeamMemberEditViewModel
    {
        public int Id { get; set; }

        [Display(Name = "الاسم بالعربي")]
        public string NameAr { get; set; }

        [Display(Name = "الاسم بالانجليزي")]
        public string NameEn { get; set; }

        [Display(Name = "الاسم بالفرنسي")]
        public string NameFr { get; set; }

        [Display(Name = "نوع العضوية")]
        public MembershipType Membership { get; set; }

        [Display(Name = "القسم")]
        public Department Department { get; set; }

        [Display(Name = "البيو بالعربي")]
        public string BioAr { get; set; }

        [Display(Name = "البيو بالانجليزي")]
        public string BioEn { get; set; }

        [Display(Name = "البيو بالفرنسي")]
        public string BioFr { get; set; }

        [Display(Name = "رفع صورة جديدة (اختياري)")]
        public IFormFile ImageFile { get; set; }

        [Display(Name = "جنس العضو")]
        public GenderType Gender { get; set; }


    }
}
