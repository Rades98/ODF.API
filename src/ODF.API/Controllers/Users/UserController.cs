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
using ODF.API.FormComposers;
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
		public async Task<IActionResult> LoginUser([FromBody] LoginUserForm form, CancellationToken cancellationToken)
		{
			var translations = await TranslationsProvider.GetTranslationsAsync(CountryCode, cancellationToken);
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

			var userResult = await Mediator.Send(new LoginUserCommand(form, CountryCode), cancellationToken);

			if (userResult.IsOk)
			{
				var claimsIdentity = new ClaimsIdentity(userResult.User.Claims, CookieAuthenticationDefaults.AuthenticationScheme);

				await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), CookieProps.AuthProps);

				var responseModel = new UserResponseModel(userResult.User.UserName, translations.Get("login_succes_title"));
				responseModel.AddAction(GetAppAction(nameof(NavigationController.GetNavigation), "nav"));

				return Ok(responseModel);
			}

			var registerAction = GetNamedAction(nameof(RegisterUser), translations.Get("register_user"), "register",
				UserFormComposer.GetRegisterForm(new(), translations));

			return Unauthorized(new UnauthorizedExceptionResponseModel(translations.Get("login_failed_title"), translations.Get("login_failed_msg"), registerAction));
		}

		[HttpPut(Name = nameof(RegisterUser))]
		[ProducesResponseType(typeof(UserRegisterResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(UserRegisterResponseModel), StatusCodes.Status422UnprocessableEntity)]
		[ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> RegisterUser([FromBody] RegisterUserForm form, CancellationToken cancellationToken)
		{
			var translations = await TranslationsProvider.GetTranslationsAsync(CountryCode, cancellationToken);

			var validationResult = await Mediator.Send(new RegisterUserCommand(form, $"{FrontEndUrl}/user-activation/{{hash}}", CountryCode), cancellationToken);

			if (validationResult.IsOk)
			{
				return Ok(new UserRegisterResponseModel(translations.Get("registration_ok")));
			}
			if (validationResult.Errors.Any())
			{
				return UnprocessableEntity(new UserRegisterResponseModel(UserFormComposer.GetRegisterForm(form, translations, validationResult.Errors)));
			}

			return InternalServerError(new ExceptionResponseModel(translations.Get("registration_failed")));
		}

		[HttpPut("activation", Name = nameof(ActivateRegistration))]
		[ProducesResponseType(typeof(UserActivationResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(UserActivationResponseModel), StatusCodes.Status422UnprocessableEntity)]
		[ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> ActivateRegistration([FromBody] ActivateUserForm form, CancellationToken cancellationToken)
		{
			var validationResult = await Mediator.Send(new ActivateUserCommand(form, CountryCode), cancellationToken);
			var translations = await TranslationsProvider.GetTranslationsAsync(CountryCode, cancellationToken);

			if (validationResult.IsOk)
			{
				return Ok(new UserActivationResponseModel(translations.Get("registration_activation_ok")));
			}

			if (validationResult.Errors.Any())
			{
				return UnprocessableEntity(new UserActivationResponseModel(UserFormComposer.GetActivateUserForm(form, validationResult.Errors)));
			}

			return InternalServerError(new ExceptionResponseModel(translations.Get("registration_activation_failed")));
		}

		[Authorize]
		[HttpDelete(Name = nameof(LogoutUser))]
		[ProducesResponseType(typeof(UserResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> LogoutUser(CancellationToken cancellationToken)
		{
			var translations = await TranslationsProvider.GetTranslationsAsync(CountryCode, cancellationToken);

			if (HttpContext.IsLoggedIn())
			{
				var responseModel = new UserResponseModel(HttpContext.GetUserName(), translations.Get("logout_succes"));
				responseModel.AddAction(GetAppAction(nameof(NavigationController.GetNavigation), "nav"));
				await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

				return Ok(responseModel);
			}

			return UnprocessableEntity(new ExceptionResponseModel(translations.Get("logout_fail")));
		}

		//change pw

		//change e-mail
	}
}
