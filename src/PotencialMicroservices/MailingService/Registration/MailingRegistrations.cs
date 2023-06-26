using MailingService.Client;
using MailingService.Client.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MailingService.Registration
{
	public static class MailingRegistrations
	{
		public static IServiceCollection UseMailingService(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddSingleton<IClientProvider, ClientProvider>();
			services.Configure<EmailSettings>(configuration.GetSection(nameof(EmailSettings)));
			services.AddTransient<IMailSender, MailSender>();

			return services;
		}
	}
}
