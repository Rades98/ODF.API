using System;
using System.Net;
using System.Net.Mail;
using MailingService.Client.Interfaces;
using MailingService.Registration;
using Microsoft.Extensions.Options;

namespace MailingService.Client
{
	internal sealed class ClientProvider : IClientProvider
	{
		public SmtpClient SmtpClient => _client;
		public MailAddress AddrFrom => _addrFrom;

		private readonly SmtpClient _client;
		private readonly MailAddress _addrFrom;

		public ClientProvider(IOptions<EmailSettings> settings)
		{
			_ = settings ?? throw new ArgumentNullException(nameof(settings));

			var mailsettings = settings.Value;

			_client = new SmtpClient("smtp.gmail.com")
			{
				Port = 587,
				EnableSsl = true,
				DeliveryMethod = SmtpDeliveryMethod.Network,
				UseDefaultCredentials = false,
				Credentials = new NetworkCredential(mailsettings.UserName, mailsettings.Password)
			};

			_addrFrom = new MailAddress(mailsettings.UserName, mailsettings.Alias);
		}

		public void Dispose()
		{
			_client?.Dispose();
			GC.SuppressFinalize(this);
		}
	}
}
