using MediatR;
using Microsoft.AspNetCore.Mvc;
using ODF.API.Registration.SettingModels;
using ODF.API.ResponseModels.About;
using ODF.API.ResponseModels.Exceptions;
using ODF.AppLayer.CQRS.Translations.Queries;

namespace ODF.API.MinimalApi
{
	public static class AboutEndpoints
	{
		public static WebApplication MapAboutEndpoints(this WebApplication app, IMediator mediator, ApiSettings apiSettings)
		{
			app.MapGet("/{countryCode}/about", async ([FromRoute] string countryCode, CancellationToken cancellationToken) =>
			{
				var aboutText = await mediator.Send(new GetTranslationQuery("Metropole Moravskoslezského kraje a její přilehlé okolí se mohou v listopadu těšit na 1. ročník festivalu Ostravské folklorní dny, které organizuje spolek FolklorOva. Akce, která se bude v centru Ostravy a městských částech konat od středy 8. do neděle 12. listopadu, má obyvatelům představit tradiční lidovou kulturu.", "about_info", countryCode), cancellationToken);
				var header = await mediator.Send(new GetTranslationQuery("Ostravo, těš se na Ostravské dny folkloru!", "about_header", countryCode), cancellationToken);

				var responseModel = new AboutResponseModel(apiSettings.ApiUrl, aboutText, header, countryCode);
				responseModel.AddAction($"/{countryCode}/articles?size=10&offset=0&pageId=0", "about_articles", HttpMethods.Get);

				return Results.Ok(responseModel);
			})
			.WithMetadata(new ProducesResponseTypeAttribute(typeof(AboutResponseModel), StatusCodes.Status200OK))
			.WithMetadata(new ProducesResponseTypeAttribute(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError));

			return app;
		}
	}
}
