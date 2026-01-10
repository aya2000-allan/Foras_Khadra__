using System;
using System.ComponentModel.DataAnnotations;

namespace Foras_Khadra.Models
{
    public class Opportunity
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string ImagePath { get; set; } // اختياري بعد الإنشاء

        public DateTime PublishDate { get; set; } = DateTime.Now;

        public string PublishedBy { get; set; }

        [Required]
        public OpportunityType Type { get; set; }

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
