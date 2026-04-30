using Foras_Khadra.Models;

namespace Foras_Khadra.Services
{
    public interface IMailService
    {
        public Task SendEmailAsync(Contact contact);
    }
}
