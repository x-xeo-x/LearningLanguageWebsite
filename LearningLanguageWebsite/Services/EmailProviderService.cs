using LearningLanguageWebsite.Interfaces;
using System.Net.Mail;
using System.Net;

namespace LearningLanguageWebsite.Services
{
    public class EmailProviderService : IEmailProvider
    {
        private readonly SmtpClient _smtpClient;
        private readonly MailAddress _fromAddress;

        public EmailProviderService(IConfiguration configuration)
        {
            var emailConfig = configuration.GetSection("Email");
            var smtpServer = emailConfig["Server"];
            var smtpPort = int.Parse(emailConfig["Port"]);
            var smtpUsername = emailConfig["Username"];
            var smtpPassword = emailConfig["Password"];
            var fromAddress = emailConfig["EmailAddress"];
            var displayName = emailConfig["DisplayName"];

            _smtpClient = new SmtpClient(smtpServer, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true
            };

            _fromAddress = new MailAddress(fromAddress, displayName);
        }

        public void SendEmail(string to, string subject, string content)
        {
            var mail = new MailMessage(_fromAddress, new MailAddress(to))
            {
                Subject = subject,
                SubjectEncoding = System.Text.Encoding.UTF8,
                Body = content,
                BodyEncoding = System.Text.Encoding.UTF8,
                IsBodyHtml = true
            };

            try
            {
                _smtpClient.SendMailAsync(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}