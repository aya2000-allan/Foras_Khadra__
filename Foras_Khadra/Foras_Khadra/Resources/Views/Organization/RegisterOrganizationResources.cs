using System.Globalization;

namespace Foras_Khadra.Resources.Views.Organization
{
    public static class RegisterOrganizationResources
    {
        public static string OrgNameRequired => Get("OrgNameRequired");
        public static string SectorRequired => Get("SectorRequired");
        public static string CountryRequired => Get("CountryRequired");
        public static string LocationRequired => Get("LocationRequired");

        public static string ContactNameRequired => Get("ContactNameRequired");
        public static string ContactEmailRequired => Get("ContactEmailRequired");
        public static string ContactEmailInvalid => Get("ContactEmailInvalid");
        public static string PhoneRequired => Get("PhoneRequired");

        public static string PasswordRequired => Get("PasswordRequired");
        public static string PasswordLength => Get("PasswordLength");
        public static string PasswordComplexity => Get("PasswordComplexity");
        public static string ConfirmPasswordRequired => Get("ConfirmPasswordRequired");
        public static string PasswordMismatch => Get("PasswordMismatch");

        public static string EmailAlreadyUsed => Get("EmailAlreadyUsed");

        public static string PhoneDigitsOnly => Get("PhoneDigitsOnly");


        private static string Get(string key)
        {
            var lang = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

            return (lang, key) switch
            {
                // ===== Organization Info =====
                ("ar", "OrgNameRequired") => "اسم المنظمة مطلوب",
                ("en", "OrgNameRequired") => "Organization name is required",
                ("fr", "OrgNameRequired") => "Le nom de l’organisation est requis",

                ("ar", "SectorRequired") => "القطاع مطلوب",
                ("en", "SectorRequired") => "Sector is required",
                ("fr", "SectorRequired") => "Le secteur est requis",

                ("ar", "CountryRequired") => "اختر الدولة",
                ("en", "CountryRequired") => "Country is required",
                ("fr", "CountryRequired") => "Le pays est requis",

                ("ar", "LocationRequired") => "الموقع مطلوب",
                ("en", "LocationRequired") => "Location is required",
                ("fr", "LocationRequired") => "L’emplacement est requis",

                // ===== Contact Info =====
                ("ar", "ContactNameRequired") => "اسم جهة الاتصال مطلوب",
                ("en", "ContactNameRequired") => "Contact person name is required",
                ("fr", "ContactNameRequired") => "Le nom du contact est requis",

                ("ar", "ContactEmailRequired") => "البريد الرسمي مطلوب",
                ("en", "ContactEmailRequired") => "Official email is required",
                ("fr", "ContactEmailRequired") => "L’adresse e-mail est requise",

                ("ar", "ContactEmailInvalid") => "البريد الإلكتروني غير صحيح",
                ("en", "ContactEmailInvalid") => "Invalid email address",
                ("fr", "ContactEmailInvalid") => "Adresse e-mail invalide",

                ("ar", "PhoneRequired") => "رقم الهاتف مطلوب",
                ("en", "PhoneRequired") => "Phone number is required",
                ("fr", "PhoneRequired") => "Le numéro de téléphone est requis",

                // ===== Password =====
                ("ar", "PasswordRequired") => "كلمة المرور مطلوبة",
                ("en", "PasswordRequired") => "Password is required",
                ("fr", "PasswordRequired") => "Mot de passe requis",

                ("ar", "PasswordLength") => "كلمة المرور يجب أن تكون على الأقل 8 أحرف",
                ("en", "PasswordLength") => "Password must be at least 8 characters",
                ("fr", "PasswordLength") => "Le mot de passe doit contenir au moins 8 caractères",

                ("ar", "PasswordComplexity") =>
                    "كلمة المرور يجب أن تحتوي على حرف كبير، حرف صغير، رقم ورمز خاص",
                ("en", "PasswordComplexity") =>
                    "Password must contain uppercase, lowercase, number and special character",
                ("fr", "PasswordComplexity") =>
                    "Le mot de passe doit contenir une majuscule, une minuscule, un chiffre et un symbole",

                ("ar", "ConfirmPasswordRequired") => "تأكيد كلمة المرور مطلوب",
                ("en", "ConfirmPasswordRequired") => "Confirm password is required",
                ("fr", "ConfirmPasswordRequired") => "La confirmation du mot de passe est requise",

                ("ar", "PasswordMismatch") => "كلمتا المرور غير متطابقتين",
                ("en", "PasswordMismatch") => "Passwords do not match",
                ("fr", "PasswordMismatch") => "Les mots de passe ne correspondent pas",

                // ===== Other =====
                ("ar", "EmailAlreadyUsed") => "هذا البريد الإلكتروني مستخدم من قبل",
                ("en", "EmailAlreadyUsed") => "This email is already in use",
                ("fr", "EmailAlreadyUsed") => "Cette adresse e-mail est déjà utilisée",

                ("ar", "PhoneDigitsOnly") => "رقم الهاتف يجب أن يحتوي على أرقام فقط ويمكن أن يبدأ بعلامة +",
                ("en", "PhoneDigitsOnly") => "Phone number must contain digits only and can start with +",
                ("fr", "PhoneDigitsOnly") => "Le numéro de téléphone doit contenir uniquement des chiffres et peut commencer par +",

                _ => key
            };
        }
    }
}
