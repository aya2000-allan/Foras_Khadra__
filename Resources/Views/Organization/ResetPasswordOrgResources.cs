using System.Globalization;

namespace Foras_Khadra.Resources.Views.Account
{
    public static class ResetPasswordOrgResources
    {
        private static string Lang =>
            CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

        public static string EmailRequired => Lang switch
        {
            "ar" => "البريد الإلكتروني مطلوب",
            "en" => "Email is required",
            "fr" => "L'e-mail est requis",
            _ => "Email is required"
        };

        public static string EmailInvalid => Lang switch
        {
            "ar" => "البريد الإلكتروني غير صالح",
            "en" => "Invalid email address",
            "fr" => "Adresse e-mail invalide",
            _ => "Invalid email address"
        };

        public static string PasswordRequired => Lang switch
        {
            "ar" => "كلمة المرور مطلوبة",
            "en" => "Password is required",
            "fr" => "Le mot de passe est requis",
            _ => "Password is required"
        };

        public static string PasswordLength => Lang switch
        {
            "ar" => "كلمة المرور يجب أن تكون على الأقل 8 أحرف",
            "en" => "Password must be at least 8 characters",
            "fr" => "Le mot de passe doit contenir au moins 8 caractères",
            _ => "Password must be at least 8 characters"
        };

        public static string PasswordComplexity => Lang switch
        {
            "ar" => "كلمة المرور يجب أن تحتوي على حرف كبير، حرف صغير، رقم ورمز خاص",
            "en" => "Password must contain uppercase, lowercase, number and special character",
            "fr" => "Le mot de passe doit contenir une majuscule, une minuscule, un chiffre et un symbole",
            _ => "Password complexity is insufficient"
        };

        public static string ConfirmPasswordRequired => Lang switch
        {
            "ar" => "تأكيد كلمة المرور مطلوب",
            "en" => "Confirm password is required",
            "fr" => "La confirmation du mot de passe est requise",
            _ => "Confirm password is required"
        };

        public static string PasswordMismatch => Lang switch
        {
            "ar" => "كلمتا المرور غير متطابقتين",
            "en" => "Passwords do not match",
            "fr" => "Les mots de passe ne correspondent pas",
            _ => "Passwords do not match"
        };
    }
}
