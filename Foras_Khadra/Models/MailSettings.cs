namespace Foras_Khadra.Models
{
 
    //To save the mail setting from appSettting.json
    public class MailSettings
    {
        public int Id { get; set; }
        public string Mail { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }

    }
}
