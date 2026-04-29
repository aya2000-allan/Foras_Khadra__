using System.ComponentModel.DataAnnotations.Schema;

namespace Foras_Khadra.Models
{
    public class ReelsRequest
    {
        public int Id { get; set; }
        public int OpportunityId { get; set; }
        [ForeignKey("OpportunityId")]
        public Opportunity Opportunity { get; set; }

        public int OrganizationId { get; set; }
        [ForeignKey("OrganizationId")]
        public Organization Organization { get; set; }

        public DateTime RequestDate { get; set; }
        public bool IsCompleted { get; set; }

        public bool IsRejected { get; set; }
        public string? RejectionReason { get; set; }

        public bool IsInProgress { get; set; } = false;
    }

}
