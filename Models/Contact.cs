using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace Foras_Khadra.Models
{
  
public class Contact
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "First name is required")]
        [StringLength(100, ErrorMessage = "First name can't exceed 100 characters ")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        [StringLength(100, ErrorMessage = "Last name can't exceed 100 characters ")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [StringLength(255, ErrorMessage = "Email can't exceed 100 characters ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Country code is required")]
        [RegularExpression(@"^\+\d{1,4}$", ErrorMessage = "Invalid country code")] // مثال: +970, +1
        public string CountryCode { get; set; }

        [RegularExpression(@"^\d{7,15}$", ErrorMessage = "Invalid phone number")]
        public string Phone { get; set; }

        [StringLength(5000, ErrorMessage = "Email can't exceed 5000 characters ")]

        public string MessageContent { get; set; }
        public DateTime SubmissionDateTime { get; set; }
        [NotMapped]
        public string? Honeypot { get; set; }
    }
}
