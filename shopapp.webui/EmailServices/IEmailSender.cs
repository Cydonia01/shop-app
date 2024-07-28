// Purpose: Interface for EmailSender class.

using System.Threading.Tasks;

namespace shopapp.webui.EmailServices
{
    public interface IEmailSender
    {
        // smtp => gmail, hotmail
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}