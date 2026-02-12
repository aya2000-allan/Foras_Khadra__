using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Foras_Khadra.Models
{
    public class OpportunityCreateVM
    {
        [Required]
        public string TitleAr { get; set; }
        [Required]
        public string TitleEn { get; set; }
        [Required]
        public string TitleFr { get; set; }

        [Required(ErrorMessage = "يجب رفع صورة للفرصة")]
        public IFormFile Image { get; set; } // هنا الصورة إجبارية

        public string PublishedBy { get; set; }

        [Required(ErrorMessage = "نوع الفرصة مطلوب")]
        public OpportunityType? Type { get; set; }

        [Required]
        public string DescriptionAr { get; set; }

        [Required]
        public string DescriptionEn { get; set; }
        [Required]
        public string DescriptionFr { get; set; }

        [Required]
        public string DetailsAr { get; set; }
        [Required]
        public string DetailsEn { get; set; }
        [Required]
        public string DetailsFr { get; set; }
        [Required]
        public List<int> AvailableCountryIds { get; set; } = new List<int>();
        [Required]
        public string EligibilityCriteriaAr { get; set; }
        [Required]
        public string EligibilityCriteriaEn { get; set; }
        [Required]
        public string EligibilityCriteriaFr { get; set; }
        [Required]
        public string BenefitsAr { get; set; }
        [Required]
        public string BenefitsEn { get; set; }
        [Required]
        public string BenefitsFr { get; set; }

        [Required]
        public string ApplyLink { get; set; }

        public List<SelectListItem> CountriesSelectList { get; set; } = new List<SelectListItem>();


    }
}
