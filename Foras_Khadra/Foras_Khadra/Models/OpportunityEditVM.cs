using Microsoft.AspNetCore.Http;

namespace Foras_Khadra.Models
{
    public class OpportunityEditVM
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public IFormFile Image { get; set; } // رفع صورة جديدة (اختياري)

        public string ImagePath { get; set; } // ← الصورة القديمة للعرض

        public string PublishedBy { get; set; }

        public OpportunityType? Type { get; set; }

        public string Description { get; set; }

        public string Details { get; set; }

        public string AvailableCountries { get; set; }

        public string EligibilityCriteria { get; set; }

        public string Benefits { get; set; }

        public string ApplyLink { get; set; }
    }
}
