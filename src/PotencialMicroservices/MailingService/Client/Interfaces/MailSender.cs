using System;
using System.IO;
using System.Net.Mail;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using MailingService.Dtos;

namespace MailingService.Client.Interfaces
{
	internal class MailSender : IMailSender
	{
		private readonly IClientProvider _provider;

		public MailSender(IClientProvider provider)
		{
			_provider = provider ?? throw new ArgumentNullException(nameof(provider));
		}

		public async Task SendRegistrationEmailAsync(RegistrationEmailDto registrationData, CancellationToken cancellationToken)
		{
			var mailMessage = new MailMessage(_provider.AddrFrom, new MailAddress(registrationData.Email, registrationData.Name));

			mailMessage.Subject = registrationData.Subject;
			mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;

			string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location!)!, @"Templates\RegistrationTemplate.html");
			string body = File.ReadAllText(path);

			string mailBody = body
								.Replace("{header}", registrationData.Header)
								.Replace("{info}", registrationData.Info)
								.Replace("{confirmButtonText}", registrationData.ConfirmButtonText)
								.Replace("{activationUrl}", registrationData.ActivationUrl.ToString())
								.Replace("{notWorkMessage}", registrationData.NotWorkMessage)
								.Replace("{regards}", registrationData.Regards)
								.Replace("{deleteNoticeIfSpam}", registrationData.DeleteNoticeIfSpam);

			mailMessage.Body = mailBody;
			mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
			mailMessage.IsBodyHtml = true;

			await _provider.SmtpClient.SendMailAsync(mailMessage);
		}
	}
}
