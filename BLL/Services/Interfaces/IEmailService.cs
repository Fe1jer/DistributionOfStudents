using MailKit.Net.Smtp;
using MimeKit;
using System.Net;

namespace BLL.Services.Interfaces
{
    public interface IEmailService
    {
        public static async void SendEmailAsync(string email, string subject, string message)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Распределение БНТУ", "distributionbntu@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text)
            {
                Text = message
            };

            using var client = new SmtpClient();
            await client.ConnectAsync("smtp.gmail.com", 465, true);
            await client.AuthenticateAsync("distributionbntu@gmail.com", "hhflqeicdkvvfhmj");
            await client.SendAsync(emailMessage);

            await client.DisconnectAsync(true);
        }
    }
}
