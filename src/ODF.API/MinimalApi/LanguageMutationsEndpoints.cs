using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ODF.API.FormFactories;
using ODF.API.Registration.SettingModels;
using ODF.API.RequestModels.Forms;
using ODF.API.ResponseModels.Common;
using ODF.API.ResponseModels.Exceptions;
using ODF.API.ResponseModels.LanguageMutations;
using ODF.API.Responses;
using ODF.AppLayer.Consts;
using ODF.AppLayer.CQRS.Translations.Commands;
using ODF.AppLayer.CQRS.Translations.Queries;
using ODF.Enums;

namespace ODF.API.MinimalApi
{
	public static class LanguageMutationsEndpoints
	{
		public static WebApplication MapLanguageMutationsEndpoints(this WebApplication app, IMediator mediator, ApiSettings apiSettings)
		{
			app.MapGet("/{countryCode}/supportedLanguages", async ([FromRoute] string countryCode, CancellationToken cancellationToken) =>
			{
				var languages = Languages.GetAll().Select(async lang =>
				{
					var actionParttext = await mediator.Send(new GetTranslationQuery("Přepnout do {0}", "app_language_switch", countryCode), cancellationToken);
					var actionName = string.Format(actionParttext, lang.GetCountryCode());
					var languageModel = new LanguageModel(lang.Name, lang.GetCountryCode());

					if (lang.GetCountryCode().ToLower() != countryCode.ToLower())
					{
						languageModel.ChangeLanguage = new(apiSettings.ApiUrl + $"/{lang.GetCountryCode()}/navigation", actionName, "nav", HttpMethods.Get);
					}

					return languageModel;
				})
				.Select(task => task.Result);

				var title = await mediator.Send(new GetTranslationQuery("Jazyk", "app_language", countryCode), cancellationToken);
				var responseModel = new LanguageResponseModel(apiSettings.ApiUrl, languages, title, countryCode);

				return responseModel;
			})
			.WithMetadata(new ProducesResponseTypeAttribute(typeof(LanguageResponseModel), StatusCodes.Status200OK))
			.WithMetadata(new ProducesResponseTypeAttribute(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError));

			app.MapGet("{countryCode}/translations", [Authorize(Roles = UserRoles.Admin)] async ([FromRoute] string countryCode, [FromQuery] int size, [FromQuery] int offset, CancellationToken cancellationToken) =>
			{
				if (countryCode.ToUpper() != Languages.Czech.GetCountryCode())
				{
					return Results.UnprocessableEntity("This action is supported for CZ language only");
				}

				var translations = await mediator.Send(new GetTranslationsQuery(countryCode, size, offset), cancellationToken);

				var responseModel = new GetTranslationsResponseModel(apiSettings.ApiUrl, countryCode, "Správa překladů", $"/translations?size={size}&offset={offset}");
				responseModel.Translations = translations.Translations.Select(tr =>
				{
					var model = new GetTranslationResponseModel(apiSettings.ApiUrl, countryCode, tr.TranslationCode, tr.Text);

					model.ChangeEnTranslation = new NamedAction($"{apiSettings.ApiUrl}/{countryCode}/translations", "AJ překlad", "transalation_en_submit", HttpMethods.Post,
						TranslationFormFactory.GetChangeTranslationForm(model.TranslationCode, model.Text, Languages.English.GetCountryCode()));

					model.ChangeDeTransaltion = new NamedAction($"{apiSettings.ApiUrl}/{countryCode}/translations", "DE překlad", "transalation_de_submit", HttpMethods.Post,
						TranslationFormFactory.GetChangeTranslationForm(model.TranslationCode, model.Text, Languages.Deutsch.GetCountryCode()));

					model.ChangeTransaltion = new NamedAction($"{apiSettings.ApiUrl}/{countryCode}/translations", "CZ hodnota", "transalation_cz_submit", HttpMethods.Post,
						TranslationFormFactory.GetChangeTranslationForm(model.TranslationCode, model.Text, Languages.Deutsch.GetCountryCode()));

					return model;
				});

				if(offset > 0)
				{
					responseModel.AddAction($"/{countryCode}/translations?size={size}&offset={offset - 1}", "translations_previous", HttpMethods.Get);
				}

				if(translations.Count > offset * size + size)
				{
					responseModel.AddAction($"/{countryCode}/translations?size={size}&offset={offset + 1}", "translations_next", HttpMethods.Get);
				}
				
				return Results.Ok(responseModel);
			})
			.WithMetadata(new ProducesResponseTypeAttribute(typeof(GetTranslationsResponseModel), StatusCodes.Status200OK))
			.WithMetadata(new ProducesResponseTypeAttribute(typeof(UnauthorizedExceptionResponseModel), StatusCodes.Status401Unauthorized));

			app.MapPost("{countryCode}/translations", [Authorize(Roles = UserRoles.Admin)] async ([FromRoute] string countryCode, [FromBody] ChangeTranslationForm form, CancellationToken cancellationToken) =>
			{
				if (countryCode.ToUpper() != Languages.Czech.GetCountryCode())
				{
					var navAction = new NamedAction(apiSettings.ApiUrl + "/cz/navigation", "Přepnout do CZ", "nav", HttpMethods.Get);
					return CustomApiResponses.BadRequest(new BadRequestExceptionResponseModel("This action is supported for CZ language only", "Switch to cz", altAction: navAction));
				}

				var result = await mediator.Send(new ModifyTransaltionCommand(form.CountryCode, form.TranslationCode, form.Text), cancellationToken);

				if (result)
				{
					var responseModel = new PostTranslationResponseModel(apiSettings.ApiUrl, countryCode,
						TranslationFormFactory.GetChangeTranslationForm(form.TranslationCode, form.Text, Languages.Deutsch.GetCountryCode()),
						$"Proměnná {form.TranslationCode} byla úspěšně přeložena pro {form.CountryCode}: {form.Text}.");

					return Results.Ok(responseModel);
				}

				return CustomApiResponses.InternalServerError(new ExceptionResponseModel($"Při překladu {form.TranslationCode} pro {form.CountryCode} na hodnotu {form.Text} došlo k chybě."));
			})
			.WithMetadata(new ProducesResponseTypeAttribute(typeof(PostTranslationResponseModel), StatusCodes.Status200OK))
			.WithMetadata(new ProducesResponseTypeAttribute(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError))
			.WithMetadata(new ProducesResponseTypeAttribute(typeof(BadRequestExceptionResponseModel), StatusCodes.Status400BadRequest))
			.WithMetadata(new ProducesResponseTypeAttribute(typeof(UnauthorizedExceptionResponseModel), StatusCodes.Status401Unauthorized));

			return app;
		}
	}
}
