using SupportSystem.Domain.Entities;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Options;

namespace SupportSystemApp.Application.Services
{
	public class EmailSettings
	{
		public string From { get; set; }
		public string SmtpServer { get; set; }
		public string Port { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
	}
	public class EmailService
	{
		private readonly EmailSettings _settings;

		public EmailService(IOptions<EmailSettings> settings)
		{
			_settings = settings.Value;
		}

		public async Task SendEmailForResetPassword(string toEmail, string resetLink)
		{
			var mail = new MailMessage();
			mail.From = new MailAddress(_settings.From);
			mail.To.Add(toEmail);
			mail.Subject = "Reset Password Link";
			mail.Body = $"To reset your password please click the link below:\n\n{resetLink}";
			mail.IsBodyHtml = false;

			using var smtp = new SmtpClient(_settings.SmtpServer, int.Parse(_settings.Port))
			{
				Credentials = new NetworkCredential(_settings.Username, _settings.Password),
				EnableSsl = true
			};

			await smtp.SendMailAsync(mail);
		}
	}
}
