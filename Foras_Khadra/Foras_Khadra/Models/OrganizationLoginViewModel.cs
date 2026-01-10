using System.ComponentModel.DataAnnotations;

namespace Foras_Khadra.Models
{
    public class OrganizationLoginViewModel
    {
        [Required(ErrorMessage = "ادخل البريد الإلكتروني")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صالح")]
        public string Email { get; set; }

        [Required(ErrorMessage = "ادخل كلمة المرور")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
