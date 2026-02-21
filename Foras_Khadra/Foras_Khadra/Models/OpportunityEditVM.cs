using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Foras_Khadra.Models
{
    public class OpportunityEditVM
    {
        public int Id { get; set; }

        public string? TitleAr { get; set; }
        public string? TitleEn { get; set; }
        public string? TitleFr { get; set; }

        public IFormFile? Image { get; set; } // اختياري في التعديل
        public string? ImagePath { get; set; } // موجود مسبقاً

        public string? PublishedBy { get; set; }
        public OpportunityType? Type { get; set; }

        public string? DescriptionAr { get; set; }
        public string? DescriptionEn { get; set; }
        public string? DescriptionFr { get; set; }

        public string? DetailsAr { get; set; }
        public string? DetailsEn { get; set; }
        public string? DetailsFr { get; set; }

        public List<int>? AvailableCountryIds { get; set; } = new List<int>();
        public string? EligibilityCriteriaAr { get; set; }
        public string? EligibilityCriteriaEn { get; set; }
        public string? EligibilityCriteriaFr { get; set; }

        public string? BenefitsAr { get; set; }
        public string? BenefitsEn { get; set; }
        public string? BenefitsFr { get; set; }

        public string? ApplyLink { get; set; }


        public DeadlineType DeadlineType { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        public IEnumerable<SelectListItem>? CountriesSelectList { get; set; } = new List<SelectListItem>();
    }
}