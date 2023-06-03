using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ODF.API.Controllers.Base;
using ODF.API.FormFactories;
using ODF.API.Registration.SettingModels;
using ODF.API.RequestModels.Navigation;
using ODF.API.ResponseModels.Common;
using ODF.API.ResponseModels.Navigation;
using ODF.AppLayer.CQRS.Translations.Queries;
using ODF.Enums;

namespace ODF.API.Controllers
{
	public class NavigationController : BaseController
	{
		public NavigationController(IMediator mediator, IOptions<ApiSettings> apiSettings) : base(mediator, apiSettings)
		{
		}

		[HttpGet(Name = nameof(GetNavigation))]
		[Authorize]
		[AllowAnonymous]
		[ProducesResponseType(typeof(NavigationResponseModel), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetNavigation([FromRoute] NavigationRequestModel requestModel, CancellationToken cancellationToken)
		{
			var responseModel = new NavigationResponseModel(ApiSettings.ApiUrl, requestModel.CountryCode);
			responseModel.AddAction($"/{requestModel.CountryCode}/about", "menu_about", HttpMethods.Get);

			string langActionName = await Mediator.Send(new GetTranslationQuery("Jazyk", "menu_lang", requestModel.CountryCode), cancellationToken);
			responseModel.LanguageMutations = new NamedAction($"{ApiSettings.ApiUrl}/{requestModel.CountryCode}/supportedLanguages", langActionName, "languageSelection", HttpMethods.Get);

			string aboutActionName = await Mediator.Send(new GetTranslationQuery("O festivalu", "menu_about", requestModel.CountryCode), cancellationToken);
			responseModel.MenuItems.Add(new NamedAction($"{ApiSettings.ApiUrl}/{requestModel.CountryCode}/about", aboutActionName, "aboutMenuItem", HttpMethods.Get));

			string associationActionName = await Mediator.Send(new GetTranslationQuery("FolklorOVA", "menu_association", requestModel.CountryCode), cancellationToken);
			responseModel.MenuItems.Add(new NamedAction($"{ApiSettings.ApiUrl}/{requestModel.CountryCode}/association", associationActionName, "associationMenuItem", HttpMethods.Get));

			string lineupActionName = await Mediator.Send(new GetTranslationQuery("Program", "menu_lineup", requestModel.CountryCode), cancellationToken);
			responseModel.MenuItems.Add(new NamedAction($"{ApiSettings.ApiUrl}/{requestModel.CountryCode}/lineup", lineupActionName, "lineupMenuItem", HttpMethods.Get));

			string ticketsActionName = await Mediator.Send(new GetTranslationQuery("Vstupenky", "menu_tickets", requestModel.CountryCode), cancellationToken);
			responseModel.MenuItems.Add(new NamedAction($"{ApiSettings.ApiUrl}/{requestModel.CountryCode}/tickets", ticketsActionName, "ticketsMenuItem", HttpMethods.Get));

			string contactsActionName = await Mediator.Send(new GetTranslationQuery("Kontakt", "menu_contacts", requestModel.CountryCode), cancellationToken);
			responseModel.MenuItems.Add(new NamedAction($"{ApiSettings.ApiUrl}/{requestModel.CountryCode}/contacts", contactsActionName, "contactMenuItem", HttpMethods.Get));

			if (!requestModel.IsLoggedIn)
			{
				string loginActionName = await Mediator.Send(new GetTranslationQuery("Přihlásit se", "login", requestModel.CountryCode), cancellationToken);
				string loginTranslation = await Mediator.Send(new GetTranslationQuery("Uživatelské jméno", "login_username", requestModel.CountryCode), cancellationToken);
				string passwordTranslation = await Mediator.Send(new GetTranslationQuery("Heslo", "login_pw", requestModel.CountryCode), cancellationToken);

				string link = $"{ApiSettings.ApiUrl}/{requestModel.CountryCode}/user";

				responseModel.LoginAction = new NamedAction(link, loginActionName, "login", HttpMethods.Post, UserFormFactory.GetLoginForm(loginTranslation, passwordTranslation));

				string password2Translation = await Mediator.Send(new GetTranslationQuery("Heslo pro kontrolu", "login_pw2", requestModel.CountryCode), cancellationToken);
				string emailTranslation = await Mediator.Send(new GetTranslationQuery("e-mail", "login_email", requestModel.CountryCode), cancellationToken);
				string firstNameTranslation = await Mediator.Send(new GetTranslationQuery("Jméno", "login_first_name", requestModel.CountryCode), cancellationToken);
				string lastNameTranslation = await Mediator.Send(new GetTranslationQuery("Příjmení", "login_last_name", requestModel.CountryCode), cancellationToken);

				string registrationActionName = await Mediator.Send(new GetTranslationQuery("Nemáte registraci? Klikněte zde!", "register_action_name", requestModel.CountryCode), cancellationToken);

				responseModel.RegisterAction = new NamedAction(ApiSettings.ApiUrl + $"/{requestModel.CountryCode}/user", registrationActionName, "register", HttpMethods.Put,
					UserFormFactory.GetRegisterForm(loginTranslation, passwordTranslation, password2Translation, emailTranslation, firstNameTranslation, lastNameTranslation));
			}
			else
			{
				string logoutActionName = await Mediator.Send(new GetTranslationQuery("Odhlásit se", "logout", requestModel.CountryCode), cancellationToken);
				responseModel.UserName = "Admin"; //mock
				responseModel.LogoutAction = new NamedAction($"{ApiSettings.ApiUrl}/{requestModel.CountryCode}/user/logout", logoutActionName, "logout", HttpMethods.Post);
			}

			if (requestModel.CountryCode.ToUpper() == Languages.Czech.GetCountryCode().ToUpper() && requestModel.IsAdmin)
			{
				string redactionActionName = await Mediator.Send(new GetTranslationQuery("Redakce", "menu_redaction", requestModel.CountryCode), cancellationToken);
				responseModel.MenuItems.Add(new NamedAction($"{ApiSettings.ApiUrl}/{requestModel.CountryCode}/redaction", redactionActionName, "redactionMenuItem", HttpMethods.Get));
			}

			return Ok(responseModel);
		}
	}
}
