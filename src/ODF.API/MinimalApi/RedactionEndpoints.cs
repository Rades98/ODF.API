using System;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ODF.API.FormFactories;
using ODF.API.Registration.SettingModels;
using ODF.API.ResponseModels.Common;
using ODF.API.ResponseModels.Exceptions;
using ODF.API.ResponseModels.Redaction;
using ODF.AppLayer.Consts;
using ODF.AppLayer.CQRS.Translations.Queries;
using ODF.Enums;

namespace ODF.API.MinimalApi
{
	public static class RedactionEndpoints
	{
		public static WebApplication MapRedactionEndpoints(this WebApplication app, IMediator mediator, ApiSettings apiSettings)
		{
			app.MapGet("{countryCode}/redaction", [Authorize(Roles = UserRoles.Admin)] async ([FromRoute] string countryCode, CancellationToken cancellationToken) =>
			{
				if (countryCode.ToUpper() != Languages.Czech.GetCountryCode())
				{
					return Results.UnprocessableEntity("This action is supported for CZ language only");
				}

				var responseModel = new RedactionResponseModel(apiSettings.ApiUrl, countryCode, "Redakce");

				var aboutTransaltion = await mediator.Send(new GetTranslationQuery("O nás", "nav_about", countryCode), cancellationToken);
				responseModel.AddAboutArticle = GetAddArticleAction(apiSettings.ApiUrl, aboutTransaltion, 0);

				var associationTranslation = await mediator.Send(new GetTranslationQuery("FolklorOVA", "nav_association", countryCode), cancellationToken);
				responseModel.AddAssociationArticle = GetAddArticleAction(apiSettings.ApiUrl, associationTranslation, 1);

				responseModel.AddLineupItem = GetAddLineupAction(apiSettings.ApiUrl, countryCode);

				responseModel.AddAction($"/{countryCode}/translations?size=20&offset=0", "translations_change", HttpMethods.Get);

				return Results.Ok(responseModel);
			})
			.WithMetadata(new ProducesResponseTypeAttribute(typeof(RedactionResponseModel), StatusCodes.Status200OK))
			.WithMetadata(new ProducesResponseTypeAttribute(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError))
			.WithMetadata(new ProducesResponseTypeAttribute(typeof(UnauthorizedExceptionResponseModel), StatusCodes.Status401Unauthorized));

			return app;
		}

		private static NamedAction GetAddArticleAction(string baseUrl, string sectionTranslation, int pageNum)
			=> new ($"{baseUrl}/articles", $"Přidat článek do {sectionTranslation}", "add_article", HttpMethods.Put,
					ArticleFormFactory.GetAddArticleForm("", $"page{pageNum}_title_{{pridej_svuj_identifikator}}", "", $"page{pageNum}_text_{{pridej_svuj_identifikator}}", 1));

		private static NamedAction GetAddLineupAction(string baseUrl, string countryCode)
			=> new ($"{baseUrl}/{countryCode}/lineup", $"Přidat item do programu", "add_lineup_item", HttpMethods.Put,
					LineupItemFormFactory.GetAddLineupItemForm("Místo", "Interpret", "Název představení", "{mesto}_{interpret}_name", "popis vystoupení", "{mesto}_{interpret}_{vystoupeni}_desc", DateTime.Now));
	}
}
