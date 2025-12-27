using Foras_Khadra.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Utils;

namespace Foras_Khadra.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;

        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmailAsync(Contact contact)
        {
            var email = new MimeMessage();

            email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));

          
            email.To.Add(MailboxAddress.Parse("suppforaskhadra@gmail.com"));

            email.ReplyTo.Add(MailboxAddress.Parse(contact.Email));

        
            email.Subject = $"New Contact Message from {contact.FirstName} {contact.LastName}";

     
            var builder = new BodyBuilder()
            {
                HtmlBody = $@"
        <p><strong>Phone:</strong> {contact.Phone}</p>
        <p><strong>Message:</strong><br />{contact.MessageContent}</p>
    "
            };

            email.Body = builder.ToMessageBody();

            
            email.MessageId = MimeUtils.GenerateMessageId();

         
            using var smtp = new SmtpClient();
            smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

            await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
