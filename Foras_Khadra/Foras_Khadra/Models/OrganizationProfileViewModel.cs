using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Foras_Khadra.ViewModels
{
    public class OrganizationProfileViewModel
    {
        public string? Name { get; set; }
        public string? Sector { get; set; }
        public string? Country { get; set; }
        public string? Location { get; set; }
        public string? ContactName { get; set; }
        public string? ContactEmail { get; set; }
        public string? Website { get; set; }
        public string? PhoneNumber { get; set; }

        public List<SelectListItem> Countries { get; set; } = new List<SelectListItem>();
    }

}
