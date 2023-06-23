using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using ODF.API.Controllers.Base;
using ODF.API.Cookies;
using ODF.API.Extensions;
using ODF.API.FormFactories;
using ODF.API.RequestModels.Forms.User;
using ODF.API.ResponseModels.Exceptions;
using ODF.API.ResponseModels.User;
using ODF.AppLayer.CQRS.User.Commands;
using ODF.AppLayer.Extensions;
using ODF.AppLayer.Services.Interfaces;
using ODF.Domain.SettingModels;

namespace ODF.API.Controllers.Users
{
	public class UserController : BaseController
	{
		public UserController(IMediator mediator, IOptions<ApiSettings> apiSettings, IActionDescriptorCollectionProvider adcp, ITranslationsProvider translationsProvider)
			: base(mediator, apiSettings, adcp, translationsProvider)
		{
		}

		[HttpPost(Name = nameof(LoginUser))]
		[ProducesResponseType(typeof(UserResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(UnauthorizedExceptionResponseModel), StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> LoginUser([FromBody] LoginUserForm form, [FromRoute] string countryCode, CancellationToken cancellationToken)
		{
			var translations = await TranslationsProvider.GetTranslationsAsync(countryCode, cancellationToken);
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

			var userResult = await Mediator.Send(new LoginUserCommand(form.UserName, form.Password, countryCode), cancellationToken);

			if (userResult.IsOk)
			{
				var claimsIdentity = new ClaimsIdentity(userResult.User.Claims, CookieAuthenticationDefaults.AuthenticationScheme);

				await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), CookieProps.AuthProps);

				var responseModel = new UserResponseModel(userResult.User.UserName, translations.Get("login_succes_title"));
				responseModel.AddAction(GetAppAction(nameof(NavigationController.GetNavigation), "nav"));

				return Ok(responseModel);
			}

			var loginAction = GetNamedAction(nameof(LoginUser), translations.Get("login_user"), "login", UserFormComposer.GetLoginForm(form, translations, errors: userResult.Errors));

			var registerAction = GetNamedAction(nameof(RegisterUser), translations.Get("register_user"), "register",
				UserFormComposer.GetRegisterForm(new(), translations));

			return Unauthorized(new UnauthorizedExceptionResponseModel(translations.Get("login_failed_title"), translations.Get("login_failed_msg"), loginAction, registerAction));
		}

		[HttpPut(Name = nameof(RegisterUser))]
		public async Task<IActionResult> RegisterUser([FromRoute] string countryCode, CancellationToken cancellationToken)
		{
			var translations = await TranslationsProvider.GetTranslationsAsync(countryCode, cancellationToken);
			return Ok(translations.Get("work_in_progress"));
		}

		[Authorize]
		[HttpDelete(Name = nameof(LogoutUser))]
		public async Task<IActionResult> LogoutUser([FromRoute] string countryCode, CancellationToken cancellationToken)
		{
			var translations = await TranslationsProvider.GetTranslationsAsync(countryCode, cancellationToken);

			if (HttpContext.IsLoggedIn())
			{
				var responseModel = new UserResponseModel(HttpContext.GetUserName(), translations.Get("logout_succes"));
				responseModel.AddAction(GetAppAction(nameof(NavigationController.GetNavigation), "nav"));
				await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

				return Ok(responseModel);
			}

			return UnprocessableEntity(translations.Get("logout_fail"));
		}
	}
}
