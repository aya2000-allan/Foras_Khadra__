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
                        MembershipType.Officer => gender == GenderType.Female ? "مسؤولة" : "مسؤول",
                        MembershipType.Deputy_Director => gender == GenderType.Female ? "نائبة المدير" : "نائب المدير",
                        MembershipType.Avisor => gender == GenderType.Female ? "مستشارة" : "مستشار",
                        _ => ""
                    };
                case "en": // إنجليزي
                    return membership switch
                    {
                        MembershipType.Manager => "Manager",
                        MembershipType.Member => "Member",
                        MembershipType.Founder => "Founder",
                        MembershipType.Coordinator => "Coordinator",
                        MembershipType.Officer => "Officer",
                        MembershipType.Deputy_Director => "Deputy Director",
                        MembershipType.Avisor => "Advisor",
                        _ => ""
                    };
                case "fr": // فرنسي
                    return membership switch
                    {
                        MembershipType.Manager => gender == GenderType.Female ? "Directrice" : "Directeur",
                        MembershipType.Member => "Membre",
                        MembershipType.Founder => gender == GenderType.Female ? "Fondatrice" : "Fondateur",
                        MembershipType.Coordinator => gender == GenderType.Female ? "Coordinatrice" : "Coordinateur",
                        MembershipType.Officer => gender == GenderType.Female ? "Officière" : "Officier",
                        MembershipType.Deputy_Director => gender == GenderType.Female ? "Directrice adjointe" : "Directeur adjoint",
                        MembershipType.Avisor => gender == GenderType.Female ? "Conseillère" : "Conseiller",
                        _ => ""
                    };
                default:
                    return membership.ToString();
            }
        }
    }
}