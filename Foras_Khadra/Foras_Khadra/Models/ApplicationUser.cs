using Microsoft.AspNetCore.Identity;
using System.Text.Json;
using System.ComponentModel.DataAnnotations.Schema;

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
        public string FullName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public string Language { get; set; } = "en";
        public DateTime CreatedAt { get; set; }

        // فقط للمنظمات
        public string? ContactEmail { get; set; }
        public string? ContactPersonName { get; set; }
        public string? OrganizationWebsite { get; set; }
        public string? OrganizationName { get; set; }
        public string? Sector { get; set; }
        public string? Country { get; set; }
        public string? Nationality { get; set; }

        // ===== الاهتمامات للفرد =====
        public string? InterestsJson { get; set; }

        [NotMapped]
        public List<string> Interests
        {
            get => string.IsNullOrEmpty(InterestsJson) ? new List<string>() : JsonSerializer.Deserialize<List<string>>(InterestsJson)!;
            set => InterestsJson = JsonSerializer.Serialize(value);
        }
    }
}