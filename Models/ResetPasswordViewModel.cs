using System.ComponentModel.DataAnnotations;
using Foras_Khadra.Resources.Views.Account;

namespace Foras_Khadra.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(
            ErrorMessageResourceType = typeof(ResetPasswordResources),
            ErrorMessageResourceName = nameof(ResetPasswordResources.EmailRequired))]
        [EmailAddress(
            ErrorMessageResourceType = typeof(ResetPasswordResources),
            ErrorMessageResourceName = nameof(ResetPasswordResources.EmailInvalid))]
        public string Email { get; set; }

        [Required]
        public string Token { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(ResetPasswordResources),
            ErrorMessageResourceName = nameof(ResetPasswordResources.PasswordRequired))]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 8,
            ErrorMessageResourceType = typeof(ResetPasswordResources),
            ErrorMessageResourceName = nameof(ResetPasswordResources.PasswordLength))]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$",
            ErrorMessageResourceType = typeof(ResetPasswordResources),
            ErrorMessageResourceName = nameof(ResetPasswordResources.PasswordComplexity))]
        public string Password { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(ResetPasswordResources),
            ErrorMessageResourceName = nameof(ResetPasswordResources.ConfirmPasswordRequired))]
        [DataType(DataType.Password)]
        [Compare("Password",
            ErrorMessageResourceType = typeof(ResetPasswordResources),
            ErrorMessageResourceName = nameof(ResetPasswordResources.PasswordMismatch))]
        public string ConfirmPassword { get; set; }
    }
}
