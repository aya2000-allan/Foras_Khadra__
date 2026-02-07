using Foras_Khadra.Resources.Views.Account;
using System.ComponentModel.DataAnnotations;

namespace Foras_Khadra.Models
{
    public class OrganizationResetPasswordViewModel
    {
        [Required(
            ErrorMessageResourceType = typeof(ResetPasswordOrgResources),
            ErrorMessageResourceName = nameof(ResetPasswordOrgResources.EmailRequired))]
        [EmailAddress(
            ErrorMessageResourceType = typeof(ResetPasswordOrgResources),
            ErrorMessageResourceName = nameof(ResetPasswordOrgResources.EmailInvalid))]
        public string Email { get; set; }
        public string Token { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(ResetPasswordOrgResources),
            ErrorMessageResourceName = nameof(ResetPasswordOrgResources.PasswordRequired))]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 8,
            ErrorMessageResourceType = typeof(ResetPasswordOrgResources),
            ErrorMessageResourceName = nameof(ResetPasswordOrgResources.PasswordLength))]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$",
            ErrorMessageResourceType = typeof(ResetPasswordOrgResources),
            ErrorMessageResourceName = nameof(ResetPasswordOrgResources.PasswordComplexity))]
        public string Password { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(ResetPasswordOrgResources),
            ErrorMessageResourceName = nameof(ResetPasswordOrgResources.ConfirmPasswordRequired))]
        [DataType(DataType.Password)]
        [Compare("Password",
            ErrorMessageResourceType = typeof(ResetPasswordOrgResources),
            ErrorMessageResourceName = nameof(ResetPasswordOrgResources.PasswordMismatch))]
        public string ConfirmPassword { get; set; }
    }

}
