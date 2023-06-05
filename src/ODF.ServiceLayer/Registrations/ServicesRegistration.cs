using Microsoft.Extensions.DependencyInjection;
using ODF.AppLayer.Services;
using ODF.ServiceLayer.Translations;

namespace ODF.ServiceLayer.Registrations
{
	public static class ServicesRegistration
	{
		public static IServiceCollection AddProjectServices(this IServiceCollection services)
		{
			services.AddTransient<ITranslationService, TranslationServices>();

			return services;
		}
	}
}
