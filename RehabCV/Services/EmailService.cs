using MailKit.Net.Smtp;
using MimeKit;
using RehabCV.Interfaces;
using System.Threading.Tasks;

namespace RehabCV.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string email, string subject, string message, 
                                         string emailOfCenter, string passwordOfEmail)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Реабілітаційний центр м. Чернівці", emailOfCenter));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, false);
                await client.AuthenticateAsync(emailOfCenter, passwordOfEmail);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
