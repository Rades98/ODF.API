using System.Threading;
using System.Threading.Tasks;
using MailingService.Dtos;

namespace MailingService.Client.Interfaces
{
	public interface IMailSender
	{
		Task SendRegistrationEmailAsync(RegistrationEmailDto registrationData, CancellationToken cancellationToken);
	}
}
