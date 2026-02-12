using System.Globalization;

namespace Foras_Khadra.Resources.Views.Account
{
    public static class RegisterOrgResources
    {
        public static string FirstNameRequired => GetMessage("FirstNameRequired");
        public static string LastNameRequired => GetMessage("LastNameRequired");
        public static string EmailRequired => GetMessage("EmailRequired");
        public static string EmailInvalid => GetMessage("EmailInvalid");
        public static string CountryRequired => GetMessage("CountryRequired");
        public static string NationalityRequired => GetMessage("NationalityRequired");
        public static string PasswordRequired => GetMessage("PasswordRequired");
        public static string PasswordLength => GetMessage("PasswordLength");
        public static string PasswordComplexity => GetMessage("PasswordComplexity");
        public static string ConfirmPasswordRequired => GetMessage("ConfirmPasswordRequired");
        public static string PasswordMismatch => GetMessage("PasswordMismatch");
        public static string SelectInterestAlert => GetMessage("SelectInterestAlert");

        private static string GetMessage(string key)
        {
            var lang = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            return (lang, key) switch
            {
                ("ar", "FirstNameRequired") => "الاسم الأول مطلوب",
                ("en", "FirstNameRequired") => "First name is required",
                ("fr", "FirstNameRequired") => "Veuillez saisir votre prénom",

                ("ar", "LastNameRequired") => "الاسم الأخير مطلوب",
                ("en", "LastNameRequired") => "Last name is required",
                ("fr", "LastNameRequired") => "Veuillez saisir votre nom",

                ("ar", "EmailRequired") => "البريد الإلكتروني مطلوب",
                ("en", "EmailRequired") => "Email is required",
                ("fr", "EmailRequired") => "Veuillez saisir votre e-mail",

                ("ar", "EmailInvalid") => "البريد الإلكتروني غير صالح",
                ("en", "EmailInvalid") => "Invalid email address",
                ("fr", "EmailInvalid") => "Adresse e-mail invalide",

                ("ar", "CountryRequired") => "الدولة مطلوبة",
                ("en", "CountryRequired") => "Country is required",
                ("fr", "CountryRequired") => "Pays requis",

                ("ar", "NationalityRequired") => "الجنسية مطلوبة",
                ("en", "NationalityRequired") => "Nationality is required",
                ("fr", "NationalityRequired") => "Nationalité requise",

                ("ar", "PasswordRequired") => "كلمة المرور مطلوبة",
                ("en", "PasswordRequired") => "Password is required",
                ("fr", "PasswordRequired") => "Mot de passe requis",

                ("ar", "PasswordLength") => "كلمة المرور يجب أن تكون على الأقل 8 أحرف",
                ("en", "PasswordLength") => "Password must be at least 8 characters",
                ("fr", "PasswordLength") => "Le mot de passe doit contenir au moins 8 caractères",

                ("ar", "PasswordComplexity") => "كلمة المرور يجب أن تحتوي على حرف كبير، حرف صغير، رقم ورمز خاص",
                ("en", "PasswordComplexity") => "Password must contain uppercase, lowercase, number, and special character",
                ("fr", "PasswordComplexity") => "Le mot de passe doit contenir une majuscule, une minuscule, un chiffre et un symbole",

                ("ar", "ConfirmPasswordRequired") => "تأكيد كلمة المرور مطلوب",
                ("en", "ConfirmPasswordRequired") => "Confirm password is required",
                ("fr", "ConfirmPasswordRequired") => "Confirmation du mot de passe requise",

                ("ar", "PasswordMismatch") => "كلمة المرور غير متطابقة",
                ("en", "PasswordMismatch") => "Passwords do not match",
                ("fr", "PasswordMismatch") => "Les mots de passe ne correspondent pas",

                ("ar", "SelectInterestAlert") => "اختر اهتمام واحد على الأقل",
                ("en", "SelectInterestAlert") => "Please select at least one interest",
                ("fr", "SelectInterestAlert") => "Veuillez sélectionner au moins un centre d'intérêt",

                _ => key
            };
        }
    }
}