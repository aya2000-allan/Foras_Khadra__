using System.ComponentModel.DataAnnotations;

namespace Foras_Khadra.Models
{
    public enum OpportunityType
    {
        [Display(Name = "المسابقات")]
        Competitions,
        [Display(Name = "المؤتمرات")]
        Conferences,
        [Display(Name = "فرص التطوع")]
        Volunteering,
        [Display(Name = "الزمالات")]
        Fellowships,
        [Display(Name = "فرص التدريب")]
        Internships,
        [Display(Name = "الوظائف")]
        Jobs,
        [Display(Name = "المنح الدراسية")]
        Scholarships
    }
}
