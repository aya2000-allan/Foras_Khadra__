namespace Foras_Khadra.Models
{
    public class ReelsRequestAdminVM
    {
        public int Id { get; set; }
        public int OpportunityId { get; set; } // لازم للزر
        public string OpportunityTitle { get; set; }
        public OpportunityType OpportunityType { get; set; }
        public string OrganizationName { get; set; }
        public DateTime RequestDate { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsRejected { get; set; }
        public string? RejectionReason { get; set; }
        public bool IsInProgress { get; set; }

    }


}
