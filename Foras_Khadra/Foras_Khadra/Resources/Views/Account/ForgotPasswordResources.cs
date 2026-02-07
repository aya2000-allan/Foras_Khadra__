using System.Globalization;

namespace Foras_Khadra.Resources.Views.Account
{
    public static class ForgotPasswordResources
    {
        public static string EmailRequired
        {
            get
            {
                var lang = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
                return lang switch
                {
                    "ar" => "الرجاء إدخال البريد الإلكتروني",
                    "en" => "Please enter your email",
                    "fr" => "Veuillez saisir votre e-mail",
                    _ => "Please enter your email"
                };
            }
        }

        public static string EmailInvalid
        {
            get
            {
                var lang = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
                return lang switch
                {
                    "ar" => "البريد الإلكتروني غير صالح",
                    "en" => "Invalid email address",
                    "fr" => "Adresse e-mail invalide",
                    _ => "Invalid email address"
                };
            }
        }

        public static string PasswordRequired
        {
            get
            {
                var lang = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
                return lang switch
                {
                    "ar" => "الرجاء إدخال كلمة المرور",
                    "en" => "Please enter your password",
                    "fr" => "Veuillez saisir votre mot de passe",
                    _ => "Please enter your password"
                };
            }
        }
    }
}
