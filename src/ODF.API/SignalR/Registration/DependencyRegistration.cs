using Microsoft.AspNetCore.SignalR;
using ODF.API.SignalR.Hubs;
using ODF.API.SignalR.Interfaces;
using ODF.API.SignalR.Providers;
using ODF.Domain.SettingModels;

namespace ODF.API.SignalR.Registration
{
	public static class DependencyRegistration
	{
		public static IServiceCollection RegisterSignalR(this IServiceCollection services)
		{
			services.AddSignalR();
			services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
			services.AddSingleton<IConsumerStatusProvider, ConsumerStatusProvider>();

			return services;
		}

		public static IEndpointRouteBuilder SetupSignalR(this IEndpointRouteBuilder app, IConfiguration configuration)
		{
			var apiSettings = configuration.GetSection(nameof(ApiSettings)).Get<ApiSettings>()
				?? throw new ArgumentException(nameof(ApiSettings));

			app.MapHub<ChatHub>(apiSettings.SignalChatHubPath);

			return app;
		}
	}
}
