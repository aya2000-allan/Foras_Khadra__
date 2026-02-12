namespace Foras_Khadra.Models
{
    public class OrgOpportunityVM
    {
        public int Id { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string TitleFr { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionFr { get; set; }
        public List<string> AvailableCountryNames { get; set; }
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
