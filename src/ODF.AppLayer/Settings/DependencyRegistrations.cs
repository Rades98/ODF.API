using System.Reflection;
using FluentValidation;
using MailingService.Registration;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ODF.AppLayer.Pipelines;
using ODF.AppLayer.Services;
using ODF.AppLayer.Services.Interfaces;

namespace ODF.AppLayer.Settings
{
	public static class DependencyRegistrations
	{
		public static IServiceCollection AddAppLayerServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.UseMailingService(configuration);
			services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
				.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), lifetime: ServiceLifetime.Transient)
				.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipeline<,>))
				.AddTransient(typeof(IRequestPostProcessor<,>), typeof(ResourceNotFoundPostProcessor<,>));

			services.AddTransient<ITranslationsProvider, TranslationsProvider>();

			return services;
		}
	}
}
