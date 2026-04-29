using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations;
using Foras_Khadra.Resources.Views.Account;
namespace Foras_Khadra.Models
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ForgotPasswordResources), ErrorMessageResourceName = "EmailRequired")]
        [EmailAddress(ErrorMessageResourceType = typeof(ForgotPasswordResources), ErrorMessageResourceName = "EmailInvalid")]
        public string Email { get; set; }
    }
}
