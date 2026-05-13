using System.ComponentModel.DataAnnotations;

namespace Foras_Khadra.Models
{
    public class ManualOrganization
    {
        public int Id { get; set; }

        public string? OrganizationName { get; set; }

        public string? ContactPersonName { get; set; }

        public string? Details { get; set; }

        public string? PhoneNumber { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string? Website { get; set; }

        public string? Location { get; set; }

        public string? Country { get; set; }

        [Required(ErrorMessage = "ادخل القطاع")]
        public string? Sector { get; set; }

        public bool IsActive { get; set; } = true;
        public string? LogoPath { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}