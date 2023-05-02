using MediatR;
using Microsoft.AspNetCore.Mvc;
using ODF.API.Registration.SettingModels;

namespace ODF.API.MinimalApi
{
	public static class LineupEndpoints
	{
		public static WebApplication MapLineupEndpoints(this WebApplication app, IMediator mediator, ApiSettings apiSettings)
		{
			app.MapGet("/{countryCode}/lineup", ([FromRoute] string countryCode, CancellationToken ct) => "Work in progress");

			return app;
		}
	}
}
