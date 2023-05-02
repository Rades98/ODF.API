using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ODF.API.Extensions;
using ODF.API.FormFactories;
using ODF.API.Registration.SettingModels;
using ODF.API.ResponseModels.Common;
using ODF.API.ResponseModels.Navigation;
using ODF.AppLayer.CQRS.Translations.Queries;
using ODF.Enums;

namespace ODF.API.MinimalApi
{
	public static class NavigationEndpoints
	{
		public static WebApplication MapNavigationEndpoints(this WebApplication app, IMediator mediator, ApiSettings apiSettings)
		{
			app.MapGet("", [Authorize][AllowAnonymous] () =>
			{
				return Results.Redirect("/cz/navigation", true, true);
			});

			app.MapGet("/{countryCode}/navigation", [Authorize][AllowAnonymous] async ([FromRoute] string countryCode, HttpContext context, CancellationToken cancellationToken) =>
			{
				var responseModel = new NavigationResponseModel(apiSettings.ApiUrl, countryCode);
				responseModel.AddAction($"/{countryCode}/about", "menu_about", HttpMethods.Get);

				var langActionName = await mediator.Send(new GetTranslationQuery("Jazyk", "menu_lang", countryCode), default);
				responseModel.LanguageMutations = new NamedAction($"{apiSettings.ApiUrl}/{countryCode}/supportedLanguages", langActionName, "languageSelection", HttpMethods.Get);

				var aboutActionName = await mediator.Send(new GetTranslationQuery("O festivalu", "menu_about", countryCode), default);
				responseModel.MenuItems.Add(new NamedAction($"{apiSettings.ApiUrl}/{countryCode}/about", aboutActionName, "aboutMenuItem", HttpMethods.Get));

				var associationActionName = await mediator.Send(new GetTranslationQuery("FolklorOVA", "menu_association", countryCode), default);
				responseModel.MenuItems.Add(new NamedAction($"{apiSettings.ApiUrl}/{countryCode}/association", associationActionName, "associationMenuItem", HttpMethods.Get));

				var lineupActionName = await mediator.Send(new GetTranslationQuery("Program", "menu_lineup", countryCode), default);
				responseModel.MenuItems.Add(new NamedAction($"{apiSettings.ApiUrl}/{countryCode}/lineup", lineupActionName, "lineupMenuItem", HttpMethods.Get));

				var ticketsActionName = await mediator.Send(new GetTranslationQuery("Vstupenky", "menu_tickets", countryCode), default);
				responseModel.MenuItems.Add(new NamedAction($"{apiSettings.ApiUrl}/{countryCode}/tickets", ticketsActionName, "ticketsMenuItem", HttpMethods.Get));

				if (!context.IsLoggedIn())
				{
					var loginActionName = await mediator.Send(new GetTranslationQuery("Přihlásit se", "login", countryCode), default);
					var loginTranslation = await mediator.Send(new GetTranslationQuery("Uživatelské jméno", "login_username", countryCode), default);
					var passwordTranslation = await mediator.Send(new GetTranslationQuery("Heslo", "login_pw", countryCode), default);

					var link = $"{apiSettings.ApiUrl}/{countryCode}/user";

					responseModel.LoginAction = new NamedAction(link, loginActionName, "login", HttpMethods.Post, UserFormFactory.GetLoginForm(loginTranslation, passwordTranslation));
				}

				if (countryCode.ToUpper() == Languages.Czech.GetCountryCode().ToUpper() && context.IsAdmin())
				{
					var redactionActionName = await mediator.Send(new GetTranslationQuery("Redakce", "menu_redaction", countryCode), default);
					responseModel.MenuItems.Add(new NamedAction($"{apiSettings.ApiUrl}/{countryCode}/redaction", redactionActionName, "redactionMenuItem", HttpMethods.Get));
				}

				return Results.Ok(responseModel);
			})
			.WithMetadata(new ProducesResponseTypeAttribute(typeof(NavigationResponseModel), StatusCodes.Status200OK));

			return app;
		}
	}
}
