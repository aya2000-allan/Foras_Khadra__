using Microsoft.AspNetCore.Identity;

namespace Foras_Khadra.Models
{
    public enum UserRole
    {
        Guest,
        User,
        Organization,
        Admin
    }

    public class ApplicationUser : IdentityUser
    {

        public string FullName { get; set; }
        public UserRole Role { get; set; }
        public string Language { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
