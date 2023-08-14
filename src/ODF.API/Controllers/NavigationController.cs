using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using ODF.API.Controllers.Base;
using ODF.API.Controllers.Contacts;
using ODF.API.Controllers.Lineup;
using ODF.API.Controllers.Users;
using ODF.API.Extensions;
using ODF.API.FormComposers;
using ODF.API.ResponseModels.Navigation;
using ODF.AppLayer.Extensions;
using ODF.AppLayer.Services.Interfaces;
using ODF.Domain;
using ODF.Domain.SettingModels;

namespace ODF.API.Controllers
{
	public class NavigationController : BaseController
	{
		public NavigationController(IMediator mediator, IOptions<ApiSettings> apiSettings,
			IActionDescriptorCollectionProvider adcp, ITranslationsProvider translationsProvider)
			: base(mediator, apiSettings, adcp, translationsProvider)
		{
		}

		[HttpGet(Name = nameof(GetNavigation))]
		[Authorize]
		[AllowAnonymous]
		[ProducesResponseType(typeof(NavigationResponseModel), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetNavigation(CancellationToken cancellationToken)
		{
			var translations = await TranslationsProvider.GetTranslationsAsync(CountryCode, cancellationToken);

			var responseModel = new NavigationResponseModel();
			responseModel.AddAction(GetAppAction(nameof(AboutController.GetAbout), "menu_about"));

			responseModel.LanguageMutations = GetNamedAction(nameof(SupportedLanguagesController.GetSupportedLanguages), translations.Get("menu_lang"), "languageSelection");

			responseModel.MenuItems.Add("aboutMenuItem", GetNamedAction(nameof(AboutController.GetAbout), translations.Get("menu_about"), translations.Get("menuitem_about")));
			responseModel.MenuItems.Add("associationMenuItem", GetNamedAction(nameof(AssociationController.GetAssociation), translations.Get("menu_association"), translations.Get("menuitem_association")));
			responseModel.MenuItems.Add("donationMenuItem", GetNamedAction(nameof(DonationController.GetDonation), translations.Get("menu_donation"), translations.Get("menuitem_donation")));
			responseModel.MenuItems.Add("contactMenuItem", GetNamedAction(nameof(ContactController.GetContacts), translations.Get("menu_contacts"), translations.Get("menuitem_contact")));
			responseModel.MenuItems.Add("lineupMenuItem", GetNamedAction(nameof(LineupController.GetLineup), translations.Get("menu_lineup"), translations.Get("menuitem_lineup")));

			responseModel.ActivateUserAction = GetAppAction(nameof(UserController.ActivateRegistration), "activate_user", UserFormComposer.GetActivateUserForm(new()));

			responseModel.SignalHubChatUrl = SignalHubUrl;

			if (!HttpContext.IsLoggedIn())
			{
				responseModel.LoginAction = GetNamedAction(nameof(UserController.LoginUser), translations.Get("login_user"), "login", UserFormComposer.GetLoginForm(new(), translations));

				responseModel.RegisterAction = GetNamedAction(nameof(UserController.RegisterUser), translations.Get("register_user"), "register",
					UserFormComposer.GetRegisterForm(new(), translations));

				responseModel.UserPageAction = GetNamedAction(nameof(UserMenuController.GetMenu), HttpContext.GetUserName(), "userMenuItem");
			}
			else
			{
				responseModel.UserName = HttpContext.GetUserName();
				responseModel.LogoutAction = GetNamedAction(nameof(UserController.LogoutUser), translations.Get("logout_user"), "logout");
			}

			if (Languages.TryParse(CountryCode, out var lang) &&
				lang is not null
				&& lang.GetCountryCode() == Languages.Czech.GetCountryCode()
				&& HttpContext.IsAdmin())
			{
				responseModel.MenuItems.Add("redactionMenuItem", GetNamedAction(nameof(RedactionController.GetRedaction), "Redakce", "fakin-redakce-dzea"));
			}

			return Ok(responseModel);
		}
	}
}
