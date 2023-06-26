using System;
using System.Net.Mail;

namespace MailingService.Client.Interfaces
{
	public interface IClientProvider : IDisposable
	{
		SmtpClient SmtpClient { get; }

		MailAddress AddrFrom { get; }
	}
}
