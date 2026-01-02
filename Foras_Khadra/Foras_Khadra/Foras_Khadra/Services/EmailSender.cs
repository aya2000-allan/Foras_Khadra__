using Microsoft.AspNetCore.Identity;
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
        var emailSettings = _configuration.GetSection("EmailSettings");

        var smtpClient = new SmtpClient(emailSettings["Host"])
        {
            Port = int.Parse(emailSettings["Port"]),
            Credentials = new NetworkCredential(emailSettings["UserName"], emailSettings["Password"]),
            EnableSsl = bool.Parse(emailSettings["EnableSSL"]),
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(emailSettings["UserName"]),
            Subject = subject,
            Body = htmlMessage,
            IsBodyHtml = true,
        };

        mailMessage.To.Add(email);

        await smtpClient.SendMailAsync(mailMessage);
    }
}
