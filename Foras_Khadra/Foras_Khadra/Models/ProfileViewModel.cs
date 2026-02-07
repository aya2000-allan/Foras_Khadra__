namespace Foras_Khadra.Models
{
    public class ProfileViewModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string Nationality { get; set; }
        public List<InterestItem> Interests { get; set; } = new();
    }
}
