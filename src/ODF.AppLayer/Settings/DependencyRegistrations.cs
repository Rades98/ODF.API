using System.Reflection;
using FluentValidation;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using ODF.AppLayer.Pipelines;

namespace ODF.AppLayer.Settings
{
	public static class DependencyRegistrations
	{
		public static IServiceCollection AddAppLayerServices(this IServiceCollection services)
		{
			services.AddMediatR(Assembly.GetExecutingAssembly())
				.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), lifetime: ServiceLifetime.Transient)
				.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipeline<,>))
				.AddTransient(typeof(IRequestPostProcessor<,>), typeof(ResourceNotFoundPostProcessor<,>));

			return services;
		}
	}
}
