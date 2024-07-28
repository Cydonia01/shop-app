// Purpose: Contains the SmtpEmailSender class which is used to send emails using the SMTP protocol.

using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace shopapp.webui.EmailServices
{
    public class SmtpEmailSender : IEmailSender
    {
        private string Host { get; set; }
        private int Port { get; set; }
        private bool EnableSSL { get; set; }
        private string Username { get; set; }
        private string Password { get; set; }

        public SmtpEmailSender(string host, int port, bool enableSSL, string username, string password)
        {
            Host = host;
            Port = port;
            EnableSSL = enableSSL;
            Username = username;
            Password = password;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Create a new SmtpClient object
            var client = new SmtpClient(Host, Port)
            {
                Credentials = new NetworkCredential(Username, Password),
                EnableSsl = EnableSSL
            };
            // Send the email
            return client.SendMailAsync(
                new MailMessage(Username, email, subject, htmlMessage) { IsBodyHtml = true }
            );
        }
    }
}