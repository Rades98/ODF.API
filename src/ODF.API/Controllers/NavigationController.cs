using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ODF.API.Extensions;
using ODF.API.FormFactories;
using ODF.API.Registration.SettingModels;
using ODF.API.ResponseModels.Common;
using ODF.API.ResponseModels.Navigation;
using ODF.AppLayer.CQRS.Translations.Queries;
using ODF.Enums;

namespace ODF.API.Controllers
{
	public class NavigationController : Controller
	{
		private readonly IMediator _mediator;
		private readonly ApiSettings _settings;

		public NavigationController(IMediator mediator, IOptions<ApiSettings> apiSettings)
		{
			_mediator = mediator;
			_settings = apiSettings.Value;
		}


		[HttpGet("")]
		[Authorize]
		[AllowAnonymous]
		public IActionResult RedirectToDefaultNavigation()
		{
			return Redirect("/cz/navigation");
		}

		[HttpGet("/{countryCode}/navigations")]
		[Authorize]
		[AllowAnonymous]
		[ProducesResponseType(typeof(NavigationResponseModel), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetNavigation([FromRoute] string countryCode, CancellationToken cancellationToken)
		{
			var responseModel = new NavigationResponseModel(_settings.ApiUrl, countryCode);
			responseModel.AddAction($"/{countryCode}/about", "menu_about", HttpMethods.Get);

			string langActionName = await _mediator.Send(new GetTranslationQuery("Jazyk", "menu_lang", countryCode), cancellationToken);
			responseModel.LanguageMutations = new NamedAction($"{_settings.ApiUrl}/{countryCode}/supportedLanguages", langActionName, "languageSelection", HttpMethods.Get);

			string aboutActionName = await _mediator.Send(new GetTranslationQuery("O festivalu", "menu_about", countryCode), cancellationToken);
			responseModel.MenuItems.Add(new NamedAction($"{_settings.ApiUrl}/{countryCode}/about", aboutActionName, "aboutMenuItem", HttpMethods.Get));

			string associationActionName = await _mediator.Send(new GetTranslationQuery("FolklorOVA", "menu_association", countryCode), cancellationToken);
			responseModel.MenuItems.Add(new NamedAction($"{_settings.ApiUrl}/{countryCode}/association", associationActionName, "associationMenuItem", HttpMethods.Get));

			string lineupActionName = await _mediator.Send(new GetTranslationQuery("Program", "menu_lineup", countryCode), cancellationToken);
			responseModel.MenuItems.Add(new NamedAction($"{_settings.ApiUrl}/{countryCode}/lineup", lineupActionName, "lineupMenuItem", HttpMethods.Get));

			string ticketsActionName = await _mediator.Send(new GetTranslationQuery("Vstupenky", "menu_tickets", countryCode), cancellationToken);
			responseModel.MenuItems.Add(new NamedAction($"{_settings.ApiUrl}/{countryCode}/tickets", ticketsActionName, "ticketsMenuItem", HttpMethods.Get));

			string contactsActionName = await _mediator.Send(new GetTranslationQuery("Kontakt", "menu_contacts", countryCode), cancellationToken);
			responseModel.MenuItems.Add(new NamedAction($"{_settings.ApiUrl}/{countryCode}/contacts", contactsActionName, "contactMenuItem", HttpMethods.Get));


			if (!HttpContext.IsLoggedIn())
			{
				string loginActionName = await _mediator.Send(new GetTranslationQuery("Přihlásit se", "login", countryCode), cancellationToken);
				string loginTranslation = await _mediator.Send(new GetTranslationQuery("Uživatelské jméno", "login_username", countryCode), cancellationToken);
				string passwordTranslation = await _mediator.Send(new GetTranslationQuery("Heslo", "login_pw", countryCode), cancellationToken);

				string link = $"{_settings.ApiUrl}/{countryCode}/user";

				responseModel.LoginAction = new NamedAction(link, loginActionName, "login", HttpMethods.Post, UserFormFactory.GetLoginForm(loginTranslation, passwordTranslation));

				string password2Translation = await _mediator.Send(new GetTranslationQuery("Heslo pro kontrolu", "login_pw2", countryCode), cancellationToken);
				string emailTranslation = await _mediator.Send(new GetTranslationQuery("e-mail", "login_email", countryCode), cancellationToken);
				string firstNameTranslation = await _mediator.Send(new GetTranslationQuery("Jméno", "login_first_name", countryCode), cancellationToken);
				string lastNameTranslation = await _mediator.Send(new GetTranslationQuery("Příjmení", "login_last_name", countryCode), cancellationToken);

				string registrationActionName = await _mediator.Send(new GetTranslationQuery("Nemáte registraci? Klikněte zde!", "register_action_name", countryCode), cancellationToken);

				responseModel.RegisterAction = new NamedAction(_settings.ApiUrl + $"/{countryCode}/user", registrationActionName, "register", HttpMethods.Put,
					UserFormFactory.GetRegisterForm(loginTranslation, passwordTranslation, password2Translation, emailTranslation, firstNameTranslation, lastNameTranslation));
			}
			else
			{
				string logoutActionName = await _mediator.Send(new GetTranslationQuery("Odhlásit se", "logout", countryCode), cancellationToken);
				responseModel.UserName = "Admin"; //mock
				responseModel.LogoutAction = new NamedAction($"{_settings.ApiUrl}/{countryCode}/user/logout", logoutActionName, "logout", HttpMethods.Post);
			}

			if (countryCode.ToUpper() == Languages.Czech.GetCountryCode().ToUpper() && HttpContext.IsAdmin())
			{
				string redactionActionName = await _mediator.Send(new GetTranslationQuery("Redakce", "menu_redaction", countryCode), cancellationToken);
				responseModel.MenuItems.Add(new NamedAction($"{_settings.ApiUrl}/{countryCode}/redaction", redactionActionName, "redactionMenuItem", HttpMethods.Get));
			}

			return Ok(responseModel);
		}
	}
}
