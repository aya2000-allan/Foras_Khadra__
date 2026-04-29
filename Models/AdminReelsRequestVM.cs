namespace Foras_Khadra.Models
{
    public class AdminReelsRequestVM
    {
        public int RequestId { get; set; }

        public string OrganizationName { get; set; }

        public string OpportunityTitle { get; set; }
        public string OpportunityType { get; set; }

        public DateTime RequestDate { get; set; }

        public bool IsCompleted { get; set; }
    }

}
