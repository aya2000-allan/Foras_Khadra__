using System.Globalization;
using Foras_Khadra.Models;

namespace Foras_Khadra.Helpers;

public static class OrgMapFilterFormatting
{
    private static readonly Dictionary<string, (string Ar, string En, string Fr)> SectorVariants =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["تعليم"] = ("تعليم", "Education", "Éducation"),
            ["التعليم"] = ("التعليم", "Education", "Éducation"),
            ["education"] = ("تعليم", "Education", "Éducation"),
            ["صحة"] = ("صحة", "Health", "Santé"),
            ["الصحة"] = ("الصحة", "Health", "Santé"),
            ["health"] = ("صحة", "Health", "Santé"),
            ["بيئة"] = ("بيئة", "Environment", "Environnement"),
            ["البيئة"] = ("البيئة", "Environment", "Environnement"),
            ["environment"] = ("بيئة", "Environment", "Environnement"),
            ["تنمية"] = ("تنمية", "Development", "Développement"),
            ["التنمية"] = ("التنمية", "Development", "Développement"),
            ["development"] = ("تنمية", "Development", "Développement"),
            ["ثقافة"] = ("ثقافة", "Culture", "Culture"),
            ["الثقافة"] = ("الثقافة", "Culture", "Culture"),
            ["culture"] = ("ثقافة", "Culture", "Culture"),
            ["رياضة"] = ("رياضة", "Sports", "Sports"),
            ["الرياضة"] = ("الرياضة", "Sports", "Sports"),
            ["sports"] = ("رياضة", "Sports", "Sports"),
            ["إغاثة"] = ("إغاثة", "Relief", "Secours"),
            ["الإغاثة"] = ("الإغاثة", "Relief", "Secours"),
            ["relief"] = ("إغاثة", "Relief", "Secours"),
            ["حقوق الإنسان"] = ("حقوق الإنسان", "Human rights", "Droits humains"),
            ["حقوق انسان"] = ("حقوق انسان", "Human rights", "Droits humains"),
            ["human rights"] = ("حقوق الإنسان", "Human rights", "Droits humains"),
            ["شباب"] = ("شباب", "Youth", "Jeunesse"),
            ["الشباب"] = ("الشباب", "Youth", "Jeunesse"),
            ["youth"] = ("شباب", "Youth", "Jeunesse"),
            ["مرأة"] = ("مرأة", "Women", "Femmes"),
            ["المرأة"] = ("المرأة", "Women", "Femmes"),
            ["women"] = ("مرأة", "Women", "Femmes"),
            ["طفولة"] = ("طفولة", "Childhood / Children", "Enfance"),
            ["الطفولة"] = ("الطفولة", "Childhood / Children", "Enfance"),
            ["children"] = ("طفولة", "Children", "Enfants"),
            ["إعلام"] = ("إعلام", "Media", "Médias"),
            ["الإعلام"] = ("الإعلام", "Media", "Médias"),
            ["media"] = ("إعلام", "Media", "Médias"),
            ["تكنولوجيا"] = ("تكنولوجيا", "Technology", "Technologie"),
            ["technology"] = ("تكنولوجيا", "Technology", "Technologie"),
            ["زراعة"] = ("زراعة", "Agriculture", "Agriculture"),
            ["الزراعة"] = ("الزراعة", "Agriculture", "Agriculture"),
            ["agriculture"] = ("زراعة", "Agriculture", "Agriculture"),
            ["تطوع"] = ("تطوع", "Volunteering", "Bénévolat"),
            ["التطوع"] = ("التطوع", "Volunteering", "Bénévolat"),
            ["volunteering"] = ("تطوع", "Volunteering", "Bénévolat"),
            ["أعمال"] = ("أعمال", "Business", "Entreprises"),
            ["الأعمال"] = ("الأعمال", "Business", "Entreprises"),
            ["business"] = ("أعمال", "Business", "Entreprises"),
        };

    public static string CountryLabel(string? stored, string lang, IReadOnlyList<Country> countries)
    {
        if (string.IsNullOrWhiteSpace(stored)) return string.Empty;
        var t = stored.Trim();

        var row = countries.FirstOrDefault(c =>
            string.Equals(c.NameAr, t, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(c.NameEn, t, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(c.NameFr, t, StringComparison.OrdinalIgnoreCase));

        if (row != null)
        {
            return lang switch
            {
                "en" => row.NameEn,
                "fr" => row.NameFr,
                _ => row.NameAr
            };
        }

        if (t.Length == 2)
        {
            try
            {
                var r = new RegionInfo(t.ToUpperInvariant());
                return lang switch
                {
                    "ar" => r.NativeName,
                    "fr" => r.EnglishName,
                    _ => r.EnglishName
                };
            }
            catch (ArgumentException)
            {
                /* ignore */
            }
        }

        return stored;
    }

    public static string SectorLabel(string? stored, string lang)
    {
        if (string.IsNullOrWhiteSpace(stored)) return string.Empty;
        var t = stored.Trim();

        if (SectorVariants.TryGetValue(t, out var tr))
        {
            return lang switch
            {
                "en" => tr.En,
                "fr" => tr.Fr,
                _ => tr.Ar
            };
        }

        return stored;
    }
}
