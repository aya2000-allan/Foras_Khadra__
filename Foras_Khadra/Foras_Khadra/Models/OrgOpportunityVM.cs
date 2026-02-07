namespace Foras_Khadra.Models
{
    public class OrgOpportunityVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AvailableCountries { get; set; }
        public OpportunityType Type { get; set; }
        public DateTime PublishDate { get; set; }
        public string ImagePath { get; set; }
        public bool HasRequestedReels { get; set; }
        public bool IsReelsCompleted { get; set; }
        public bool IsReelsRejected { get; set; }           
        public string RejectionReason { get; set; }
        public bool IsReelsInProgress { get; set; }

    }
}
