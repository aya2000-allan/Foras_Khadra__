using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Foras_Khadra.Models
{
    public class OpportunityCreateVM
    {
        [Required]
        public string Title { get; set; }

        [Required(ErrorMessage = "يجب رفع صورة للفرصة")]
        public IFormFile Image { get; set; } // هنا الصورة إجبارية

        public string PublishedBy { get; set; }

        [Required(ErrorMessage = "نوع الفرصة مطلوب")]
        public OpportunityType? Type { get; set; }

        [Required]
        public string Description { get; set; }

        public string Details { get; set; }

        public string AvailableCountries { get; set; }

        public string EligibilityCriteria { get; set; }

        public string Benefits { get; set; }

        [Required]
        public string ApplyLink { get; set; }
    }
}
