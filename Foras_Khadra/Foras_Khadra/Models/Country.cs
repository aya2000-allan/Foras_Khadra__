using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Foras_Khadra.Models
{
    public class Country
    {
        public int Id { get; set; }

        [Required]
        public string NameAr { get; set; }

        [Required]
        public string NameEn { get; set; }

        [Required]
        public string NameFr { get; set; }

        public virtual ICollection<Opportunity> Opportunities { get; set; }
    }
}
