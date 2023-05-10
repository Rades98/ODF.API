using System.Data;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ODF.API.Registration.SettingModels;
using ODF.API.RequestModels.Forms;
using ODF.API.ResponseModels.Exceptions;
using ODF.API.ResponseModels.Lineup;
using ODF.API.Responses;
using ODF.AppLayer.Consts;
using ODF.AppLayer.CQRS.Lineup.Commands;
using ODF.AppLayer.CQRS.Lineup.Queries;

namespace ODF.API.MinimalApi
{
	public static class LineupEndpoints
	{
		public static WebApplication MapLineupEndpoints(this WebApplication app, IMediator mediator, ApiSettings apiSettings)
		{
			app.MapGet("/{countryCode}/lineup", async ([FromRoute] string countryCode, CancellationToken cancellationToken) =>
			{
				var result = await mediator.Send(new GetLineupQuery(countryCode), cancellationToken);

				var responseModel = new LineupResponseModel(apiSettings.ApiUrl, countryCode);

				responseModel.Lineup = result.OrderBy(ord => ord.DateTime)
				 .GroupBy(o => o.Place)
				.ToDictionary(val => val.Key, val => val.Select(x => new LineupItemResponseModel()
				{
					Date = x.DateTime.ToString("dd.MM.yyyy"),
					Description = x.Description,
					Interpret = x.Interpret,
					PerformanceName = x.PerformanceName,
					Time = x.DateTime.ToString("HH:mm")
				}));

				return Results.Ok(responseModel);
			})
			.WithMetadata(new ProducesResponseTypeAttribute(typeof(LineupResponseModel), StatusCodes.Status200OK))
			.WithMetadata(new ProducesResponseTypeAttribute(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError));

			app.MapPut("/{countryCode}/lineup", [Authorize(Roles = UserRoles.Admin)] async ([FromRoute] string countryCode, [FromBody] AddLineupItemForm model, CancellationToken cancellationToken) =>
			{
				var result = await mediator.Send(new AddLineupItemCommand(
					model.Place, model.Interpret, model.PerformanceName,
					model.Description, model.DescriptionTranslationCode, model.DateTime, countryCode), cancellationToken);

				if (result)
				{
					return Results.Accepted();
				}

				return CustomApiResponses.InternalServerError(new ExceptionResponseModel("Vyskytla se chyba při tvorbě článku"));
			})
			.WithMetadata(new ProducesResponseTypeAttribute(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError))
			.WithMetadata(new ProducesResponseTypeAttribute(StatusCodes.Status202Accepted));

			return app;
		}
	}
}
