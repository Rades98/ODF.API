using MediatR;
using Microsoft.AspNetCore.Mvc;
using ODF.API.Registration.SettingModels;

namespace ODF.API.MinimalApi
{
	public static class TicketsEndpoints
	{
		public static WebApplication MapTicketsEndpoints(this WebApplication app, IMediator mediator, ApiSettings apiSettings)
		{
			app.MapGet("/{countryCode}/tickets", ([FromRoute] string countryCode, CancellationToken ct) => 
			{

			});

			return app;
		}
	}
}
