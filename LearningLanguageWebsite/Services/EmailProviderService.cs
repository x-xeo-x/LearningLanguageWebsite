﻿using LearningLanguageWebsite.Interfaces;
using System.Net.Mail;

namespace LearningLanguageWebsite.Services
{
	public class EmailProviderService : IEmailProvider
	{
		private SmtpClient _smtpClient;
		private MailAddress _fromAddress;

		public EmailProviderService(IConfiguration configuration)
		{
			_smtpClient = new SmtpClient(configuration["Email:Server"], int.Parse(configuration["Email:Port"]));
			_smtpClient.Credentials = new System.Net.NetworkCredential(configuration["Email:Username"], configuration["Email:Password"]);
            _smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

            _fromAddress = new MailAddress(configuration["Email:EmailAddress"], configuration["Email:DisplayName"]);
		}

		public void SendEmail(string to, string subject, string content)
		{
			MailMessage mail = new MailMessage();
			mail.Subject = subject;
			mail.SubjectEncoding = System.Text.Encoding.UTF8;
			mail.Body = content;
			mail.BodyEncoding = System.Text.Encoding.UTF8;
			mail.From = _fromAddress;

			mail.To.Add(new MailAddress(to));

			_smtpClient.SendAsync(mail, null);
		}
	}
}
