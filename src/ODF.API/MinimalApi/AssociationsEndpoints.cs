using MediatR;
using Microsoft.AspNetCore.Mvc;
using ODF.API.Registration.SettingModels;
using ODF.API.ResponseModels.Association;
using ODF.API.ResponseModels.Exceptions;
using ODF.AppLayer.CQRS.Translations.Queries;

namespace ODF.API.MinimalApi
{
	public static class AssociationsEndpoints
	{
		public static WebApplication MapAssociationsEndpoints(this WebApplication app, IMediator mediator, ApiSettings apiSettings)
		{
			app.MapGet("/{countryCode}/association", async ([FromRoute] string countryCode, CancellationToken ct) =>
			{
				var aboutText = await mediator.Send(new GetTranslationQuery("Jsme FolklorOVA, spolek nadšenců, kteří chtějí podporovat a dále rozvíjet kulturu v Ostravě a jejím okolí. Skrz akci Ostravské dny folkloru chceme Ostravanům ukázat tradiční lidovou kulturu a věříme, že je nadchne stejně jako nás. Lidová kultura a folklor nezná hranic je tu pro všechny, malé i velké, stejně jako pro staré i mladé.\nFolklor spojuje!", "association_info", countryCode), ct);
				var header = await mediator.Send(new GetTranslationQuery("O nás", "association_header", countryCode), ct);
				var responseModel = new AssociationResponseModel(apiSettings.ApiUrl, aboutText, header, countryCode);

				responseModel.AddAction($"/{countryCode}/articles?size=10&offset=0&pageId=1", "association_articles", HttpMethods.Get);

				return Results.Ok(responseModel);
			})
			.WithMetadata(new ProducesResponseTypeAttribute(typeof(AssociationResponseModel), StatusCodes.Status200OK))
			.WithMetadata(new ProducesResponseTypeAttribute(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError));

			return app;
		}
	}
}
