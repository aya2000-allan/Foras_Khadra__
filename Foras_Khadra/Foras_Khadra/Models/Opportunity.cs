using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foras_Khadra.Models
{
    public class Opportunity
    {
        public int Id { get; set; }

        [Required]
        public string TitleAr { get; set; }

        [Required]
        public string TitleEn { get; set; }

        [Required]
        public string TitleFr { get; set; }

        public string ImagePath { get; set; } // اختياري بعد الإنشاء

        public DateTime PublishDate { get; set; } = DateTime.Now;

        public string PublishedBy { get; set; }

        [Required]
        public OpportunityType Type { get; set; }

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
        public virtual ICollection<Country> AvailableCountries { get; set; }

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

        [Required]
        public DeadlineType DeadlineType { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
        public string CreatedByUserId { get; set; }

        [ForeignKey("CreatedByUserId")]
        public ApplicationUser CreatedByUser { get; set; }

        [NotMapped]
        public bool IsPublishedByOrganization { get; set; }

        [NotMapped]
        public bool IsPublishedByAdmin { get; set; }

        public bool IsExpired { get; set; }
        public virtual ICollection<ReelsRequest> ReelsRequests { get; set; }

        [NotMapped]
        public string AvailableCountriesNames
        {
            get
            {
                var lang = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

                if (AvailableCountries == null || !AvailableCountries.Any())
                    return string.Empty;

                return string.Join(", ", AvailableCountries.Select(c => lang switch
                {
                    "en" => c.NameEn,
                    "fr" => c.NameFr,
                    _ => c.NameAr
                }));
            }
        }

        

    }
}
