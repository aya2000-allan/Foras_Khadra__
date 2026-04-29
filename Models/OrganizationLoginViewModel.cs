using Foras_Khadra.Resources.Views.Organization;
using System.ComponentModel.DataAnnotations;

namespace Foras_Khadra.Models
{
    public class OrganizationLoginViewModel
    {
        [Required(ErrorMessageResourceType = typeof(LoginResources), ErrorMessageResourceName = "EmailRequired")]
        [EmailAddress(ErrorMessageResourceType = typeof(LoginResources), ErrorMessageResourceName = "EmailInvalid")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(LoginResources), ErrorMessageResourceName = "PasswordRequired")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
