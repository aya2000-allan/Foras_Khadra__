using System.ComponentModel.DataAnnotations;
using Foras_Khadra.Resources.Views.Account;
namespace Foras_Khadra.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessageResourceType = typeof(LoginResources), ErrorMessageResourceName = "EmailRequired")]
        [EmailAddress(ErrorMessageResourceType = typeof(LoginResources), ErrorMessageResourceName = "EmailInvalid")]
        public string Email { get; set; }
        [Required(ErrorMessageResourceType = typeof(LoginResources), ErrorMessageResourceName = "PasswordRequired")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
