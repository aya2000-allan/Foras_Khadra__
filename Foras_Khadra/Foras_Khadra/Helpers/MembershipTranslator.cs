using Foras_Khadra.Models;

namespace Foras_Khadra.Helpers
{
    public static class MembershipTranslator
    {
        public static string Translate(MembershipType membership, GenderType gender, string culture)
        {
            culture = culture.ToLower();

            switch (culture)
            {
                case "ar": // عربي
                    return membership switch
                    {
                        MembershipType.Manager => gender == GenderType.Female ? "مديرة" : "مدير",
                        MembershipType.Member => gender == GenderType.Female ? "عضوة" : "عضو",
                        MembershipType.Founder => gender == GenderType.Female ? "مؤسسة" : "مؤسس",
                        MembershipType.Coordinator => gender == GenderType.Female ? "منسقة" : "منسق",
                        _ => ""
                    };
                case "en": // إنجليزي
                    return membership switch
                    {
                        MembershipType.Manager => "Manager",
                        MembershipType.Member => "Member",
                        MembershipType.Founder => "Founder",
                        MembershipType.Coordinator => "Coordinator",
                        _ => ""
                    };
                case "fr": // فرنسي
                    return membership switch
                    {
                        MembershipType.Manager => gender == GenderType.Female ? "Directrice" : "Directeur",
                        MembershipType.Member => "Membre",
                        MembershipType.Founder => gender == GenderType.Female ? "Fondatrice" : "Fondateur",
                        MembershipType.Coordinator => gender == GenderType.Female ? "Coordinatrice" : "Coordinateur",
                        _ => ""
                    };
                default:
                    return membership.ToString();
            }
        }
    }
}
