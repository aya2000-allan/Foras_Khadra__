using Foras_Khadra.Models;  // << مهم جدًا
namespace Foras_Khadra.Helpers
{
    public static class OpportunityTypeExtensions
    {
        public static string GetDisplayName(this OpportunityType type, string lang)
        {
            return type switch
            {
                OpportunityType.Competitions => lang switch
                {
                    "en" => "Competitions",
                    "fr" => "Concours",
                    _ => "المسابقات"
                },
                OpportunityType.Conferences => lang switch
                {
                    "en" => "Conferences",
                    "fr" => "Conférences",
                    _ => "المؤتمرات"
                },
                OpportunityType.Volunteering => lang switch
                {
                    "en" => "Volunteering",
                    "fr" => "Bénévolat",
                    _ => "فرص التطوع"
                },
                OpportunityType.Fellowships => lang switch
                {
                    "en" => "Fellowships",
                    "fr" => "Bourses",
                    _ => "الزمالات"
                },
                OpportunityType.Internships => lang switch
                {
                    "en" => "Internships",
                    "fr" => "Stages",
                    _ => "فرص التدريب"
                },
                OpportunityType.Jobs => lang switch
                {
                    "en" => "Jobs",
                    "fr" => "Emplois",
                    _ => "الوظائف"
                },
                OpportunityType.Scholarships => lang switch
                {
                    "en" => "Scholarships",
                    "fr" => "Bourses d'études",
                    _ => "المنح الدراسية"
                },
                _ => type.ToString()
            };
        }
    }
}
