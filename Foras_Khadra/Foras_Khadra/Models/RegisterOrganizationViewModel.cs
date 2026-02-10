using Foras_Khadra.Resources.Views.Organization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Foras_Khadra.ViewModels
{
    public class RegisterOrganizationViewModel
    {

        // ===== Organization Info =====
        [Required(
            ErrorMessageResourceType = typeof(RegisterOrganizationResources),
            ErrorMessageResourceName = nameof(RegisterOrganizationResources.OrgNameRequired)
        )]
        public string Name { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(RegisterOrganizationResources),
            ErrorMessageResourceName = nameof(RegisterOrganizationResources.SectorRequired)
        )]
        public string Sector { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(RegisterOrganizationResources),
            ErrorMessageResourceName = nameof(RegisterOrganizationResources.CountryRequired)
        )]
        public string Country { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(RegisterOrganizationResources),
            ErrorMessageResourceName = nameof(RegisterOrganizationResources.LocationRequired)
        )]
        public string Location { get; set; }

        // ===== Contact Info =====
        [Required(
            ErrorMessageResourceType = typeof(RegisterOrganizationResources),
            ErrorMessageResourceName = nameof(RegisterOrganizationResources.ContactNameRequired)
        )]
        public string ContactName { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(RegisterOrganizationResources),
            ErrorMessageResourceName = nameof(RegisterOrganizationResources.ContactEmailRequired)
        )]
        [EmailAddress(
            ErrorMessageResourceType = typeof(RegisterOrganizationResources),
            ErrorMessageResourceName = nameof(RegisterOrganizationResources.ContactEmailInvalid)
        )]
        public string ContactEmail { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(RegisterOrganizationResources),
            ErrorMessageResourceName = nameof(RegisterOrganizationResources.PhoneRequired)
        )]
        public string? PhoneNumber { get; set; }

        // ===== Password =====
        [Required(
            ErrorMessageResourceType = typeof(RegisterOrganizationResources),
            ErrorMessageResourceName = nameof(RegisterOrganizationResources.PasswordRequired)
        )]
        [StringLength(100, MinimumLength = 8,
            ErrorMessageResourceType = typeof(RegisterOrganizationResources),
            ErrorMessageResourceName = nameof(RegisterOrganizationResources.PasswordLength)
        )]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$",
            ErrorMessageResourceType = typeof(RegisterOrganizationResources),
            ErrorMessageResourceName = nameof(RegisterOrganizationResources.PasswordComplexity)
        )]
        public string Password { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(RegisterOrganizationResources),
            ErrorMessageResourceName = nameof(RegisterOrganizationResources.ConfirmPasswordRequired)
        )]
        [Compare("Password",
            ErrorMessageResourceType = typeof(RegisterOrganizationResources),
            ErrorMessageResourceName = nameof(RegisterOrganizationResources.PasswordMismatch)
        )]
        public string ConfirmPassword { get; set; }

        // ===== Optional =====
        public string? Website { get; set; }

        // ===== Lists =====
        public List<SelectListItem> Countries { get; set; } = new();
    }
}
