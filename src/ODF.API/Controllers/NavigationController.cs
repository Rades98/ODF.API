using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using ODF.API.Controllers.Base;
using ODF.API.Controllers.Contacts;
using ODF.API.Controllers.Lineup;
using ODF.API.Controllers.Users;
using ODF.API.FormFactories;
using ODF.API.RequestModels.Navigation;
using ODF.API.ResponseModels.Navigation;
using ODF.AppLayer.Extensions;
using ODF.AppLayer.Services.Interfaces;
using ODF.Domain;
using ODF.Domain.SettingModels;

namespace ODF.API.Controllers
{
	public class NavigationController : BaseController
	{
		public NavigationController(IMediator mediator, IOptions<ApiSettings> apiSettings, IActionDescriptorCollectionProvider adcp, ITranslationsProvider translationsProvider)
			: base(mediator, apiSettings, adcp, translationsProvider)
		{
		}

		[HttpGet(Name = nameof(GetNavigation))]
		[Authorize]
		[AllowAnonymous]
		[ProducesResponseType(typeof(NavigationResponseModel), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetNavigation([FromRoute] NavigationRequestModel requestModel, CancellationToken cancellationToken)
		{
			var translations = await TranslationsProvider.GetTranslationsAsync(requestModel.CountryCode, cancellationToken);

			var responseModel = new NavigationResponseModel();
			responseModel.AddAction(GetAppAction(nameof(AboutController.GetAbout), "menu_about"));

			responseModel.LanguageMutations = GetNamedAction(nameof(SupportedLanguagesController.GetSupportedLanguages), translations.Get("menu_lang"), "languageSelection");

			responseModel.MenuItems.Add(GetNamedAction(nameof(AboutController.GetAbout), translations.Get("menu_about"), "aboutMenuItem"));
			responseModel.MenuItems.Add(GetNamedAction(nameof(AssociationController.GetAssociation), translations.Get("menu_association"), "associationMenuItem"));
			responseModel.MenuItems.Add(GetNamedAction(nameof(LineupController.GetLineup), translations.Get("menu_lineup"), "lineupMenuItem"));
			responseModel.MenuItems.Add(GetNamedAction(nameof(DonationController.GetDonation), translations.Get("menu_tickets"), "ticketsMenuItem"));
			responseModel.MenuItems.Add(GetNamedAction(nameof(ContactsController.GetContacts), translations.Get("menu_contacts"), "contactMenuItem"));

			if (!requestModel.IsLoggedIn)
			{
				responseModel.LoginAction = GetNamedAction(nameof(UsersController.LoginUser), translations.Get("login_user"), "login", UserFormFactory.GetLoginForm(new(), translations));

				responseModel.RegisterAction = GetNamedAction(nameof(UsersController.RegisterUser), translations.Get("register_user"), "register",
					UserFormFactory.GetRegisterForm(new(), translations));
			}
			else
			{
				responseModel.UserName = requestModel.UserName;
				responseModel.LogoutAction = GetNamedAction(nameof(UsersController.LogoutUser), translations.Get("logout_user"), "logout");
			}

			if (requestModel.CountryCode.ToUpper() == Languages.Czech.GetCountryCode().ToUpper() && requestModel.IsAdmin)
			{
				responseModel.MenuItems.Add(GetNamedAction(nameof(RedactionController.GetRedaction), "Redakce", "redactionMenuItem"));
			}

			return Ok(responseModel);
		}
	}
}
