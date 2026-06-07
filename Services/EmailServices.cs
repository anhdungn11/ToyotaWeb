using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ToyotaWeb.Services
{
    public class EmailService : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var fromEmail = "nanhdung840@gmail.com";
            var password = "nbokildqokocbfwd"; // 🔥 thay bằng app password

            var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(fromEmail, password),
                EnableSsl = true
            };

            var message = new MailMessage(fromEmail, email, subject, htmlMessage);
            message.IsBodyHtml = true;

            await smtp.SendMailAsync(message);
        }
    }
}