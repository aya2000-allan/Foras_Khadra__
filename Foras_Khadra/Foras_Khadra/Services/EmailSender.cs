using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

public class EmailSender : IEmailSender
{
    private readonly IConfiguration _configuration;

    public EmailSender(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        if (string.IsNullOrEmpty(email))
            throw new ArgumentException("البريد الإلكتروني المستلم فارغ");

        var mailSettings = _configuration.GetSection("MailSettings");
        string senderEmail = mailSettings["Mail"];
        string displayName = mailSettings["DisplayName"];
        string host = mailSettings["Host"];
        string portStr = mailSettings["Port"];
        string password = mailSettings["Password"];
        bool enableSSL = bool.Parse(mailSettings["EnableSSL"] ?? "true");

        if (string.IsNullOrEmpty(senderEmail) || string.IsNullOrEmpty(host) || string.IsNullOrEmpty(portStr) || string.IsNullOrEmpty(password))
            throw new Exception("MailSettings ناقص في appsettings.json");

        int port = int.Parse(portStr);

        using var smtpClient = new SmtpClient(host, port)
        {
            Credentials = new NetworkCredential(senderEmail, password),
            EnableSsl = enableSSL
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(senderEmail, displayName),
            Subject = subject,
            Body = htmlMessage,
            IsBodyHtml = true
        };

        mailMessage.To.Add(email);

        await smtpClient.SendMailAsync(mailMessage);
    }
}
