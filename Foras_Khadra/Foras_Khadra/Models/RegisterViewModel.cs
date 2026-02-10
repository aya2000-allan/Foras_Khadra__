using Foras_Khadra.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Foras_Khadra.Resources.Views.Account;

namespace Foras_Khadra.ViewModels
{
    public class RegisterViewModel
    {
        [Required(
            ErrorMessageResourceType = typeof(RegisterOrgResources),
            ErrorMessageResourceName = nameof(RegisterOrgResources.FirstNameRequired)
        )]
        public string FirstName { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(RegisterOrgResources),
            ErrorMessageResourceName = nameof(RegisterOrgResources.LastNameRequired)
        )]
        public string LastName { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(RegisterOrgResources),
            ErrorMessageResourceName = nameof(RegisterOrgResources.EmailRequired)
        )]
        [EmailAddress(
            ErrorMessageResourceType = typeof(RegisterOrgResources),
            ErrorMessageResourceName = nameof(RegisterOrgResources.EmailInvalid)
        )]
        public string Email { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(RegisterOrgResources),
            ErrorMessageResourceName = nameof(RegisterOrgResources.CountryRequired)
        )]
        public string Country { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(RegisterOrgResources),
            ErrorMessageResourceName = nameof(RegisterOrgResources.NationalityRequired)
        )]
        public string Nationality { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(RegisterOrgResources),
            ErrorMessageResourceName = nameof(RegisterOrgResources.PasswordRequired)
        )]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 8,
            ErrorMessageResourceType = typeof(RegisterOrgResources),
            ErrorMessageResourceName = nameof(RegisterOrgResources.PasswordLength)
        )]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$",
            ErrorMessageResourceType = typeof(RegisterOrgResources),
            ErrorMessageResourceName = nameof(RegisterOrgResources.PasswordComplexity)
        )]
        public string Password { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(RegisterOrgResources),
            ErrorMessageResourceName = nameof(RegisterOrgResources.ConfirmPasswordRequired)
        )]
        [DataType(DataType.Password)]
        [Compare("Password",
            ErrorMessageResourceType = typeof(RegisterOrgResources),
            ErrorMessageResourceName = nameof(RegisterOrgResources.PasswordMismatch)
        )]
        public string ConfirmPassword { get; set; }

        [MinLength(1,
            ErrorMessageResourceType = typeof(RegisterOrgResources),
            ErrorMessageResourceName = nameof(RegisterOrgResources.SelectInterestAlert)
        )]
        public List<string> Interests { get; set; } = new List<string>();

        public List<InterestItem> AvailableInterests { get; set; } = new List<InterestItem>
        {
            new InterestItem { Key = "competitions", DisplayName = "المسابقات" },
            new InterestItem { Key = "conferences", DisplayName = "المؤتمرات" },
            new InterestItem { Key = "volunteer_opportunities", DisplayName = "فرص التطوع" },
            new InterestItem { Key = "jobs", DisplayName = "الوظائف" },
            new InterestItem { Key = "grants", DisplayName = "المنح" },
            new InterestItem { Key = "fellowships", DisplayName = "الزمالات" },
            new InterestItem { Key = "training_opportunities", DisplayName = "فرص التدريب" }
        };

        public List<string> Countries { get; set; } = new List<string>();
        public List<string> Nationalities { get; set; } = new List<string>();
    }
}