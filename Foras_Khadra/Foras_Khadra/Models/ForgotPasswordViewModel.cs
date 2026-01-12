using System.ComponentModel.DataAnnotations;

namespace Foras_Khadra.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
