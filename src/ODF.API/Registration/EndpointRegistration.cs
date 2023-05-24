using MediatR;
using Microsoft.Extensions.Options;
using ODF.API.MinimalApi;
using ODF.API.Registration.SettingModels;

namespace ODF.API.Registration
{
	public static class EndpointRegistration
	{
		public static WebApplication MapApiEndpoints(this WebApplication app)
		{
			var mediator = app.Services.GetService<IMediator>() ?? throw new ArgumentNullException(nameof(IMediator));
			var settings = app.Services.GetService<IOptions<ApiSettings>>() ?? throw new ArgumentNullException(nameof(IOptions<ApiSettings>));

			app.MapNavigationEndpoints(mediator, settings.Value)
				.MapAboutEndpoints(mediator, settings.Value)
				.MapLanguageMutationsEndpoints(mediator, settings.Value)
				.MapAssociationsEndpoints(mediator, settings.Value)
				.MapLineupEndpoints(mediator, settings.Value)
				.MapTicketsEndpoints(mediator, settings.Value)
				.MapArticlesEndpoints(mediator, settings.Value)
				.MapRedactionEndpoints(mediator, settings.Value)
				.MapUserEndpoints(mediator, settings.Value)
				.MapContactsEndpoints(mediator, settings.Value);

			return app;
		}
	}
}
