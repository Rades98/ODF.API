using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using ODF.API.Controllers.Base;
using ODF.API.Extensions;
using ODF.API.FormFactories;
using ODF.API.Registration.SettingModels;
using ODF.API.RequestModels.Forms;
using ODF.API.ResponseModels.Exceptions;
using ODF.API.ResponseModels.User;
using ODF.API.Responses;
using ODF.AppLayer.CQRS.User.Commands;
using ODF.AppLayer.Extensions;
using ODF.AppLayer.Services.Interfaces;

namespace ODF.API.Controllers
{
	public class UsersController : BaseController
	{
		public UsersController(IMediator mediator, IOptions<ApiSettings> apiSettings, IActionDescriptorCollectionProvider adcp, ITranslationsProvider translationsProvider) : base(mediator, apiSettings, adcp, translationsProvider)
		{
		}

		[HttpPost(Name = nameof(LoginUser))]
		[ProducesResponseType(typeof(UserResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(UnauthorizedExceptionResponseModel), StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> LoginUser([FromBody] UserRequestForm user, [FromRoute] string countryCode, CancellationToken cancellationToken)
		{
			var translations = await TranslationsProvider.GetTranslationsAsync(countryCode, cancellationToken);
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

			var userResult = await Mediator.Send(new LoginUserCommand(user.UserName, user.Password), cancellationToken);

			if (userResult.IsOk)
			{
				var claimsIdentity = new ClaimsIdentity(userResult.User.Claims, CookieAuthenticationDefaults.AuthenticationScheme);

				var authProperties = new AuthenticationProperties
				{
					AllowRefresh = true,
					ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1),
					IsPersistent = true,
				};

				await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

				var responseModel = new UserResponseModel(userResult.User.UserName, UserFormFactory.GetLoginForm(translations));
				responseModel.AddAction(GetAppAction(nameof(NavigationController.GetNavigation), "nav"));

				return Ok(responseModel);
			}

			var loginAction = GetNamedAction(nameof(LoginUser), translations.Get("login_user"), "login", UserFormFactory.GetLoginForm(translations, errors: userResult.Errors));

			var registerAction = GetNamedAction(nameof(RegisterUser), translations.Get("register_user"), "register",
				UserFormFactory.GetRegisterForm(translations));

			return CustomApiResponses.Unauthorized(new UnauthorizedExceptionResponseModel(translations.Get("login_failed_title"), translations.Get("login_failed_msg"), loginAction, registerAction));
		}

		[HttpPut(Name = nameof(RegisterUser))]
		public IActionResult RegisterUser([FromRoute] string countryCode)
		{
			return Ok();
		}

		[HttpDelete(Name = nameof(LogoutUser))]
		public async Task<IActionResult> LogoutUser([FromRoute] string countryCode, CancellationToken cancellationToken)
		{
			if (HttpContext.IsLoggedIn())
			{
				await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
				return Accepted();
			}

			return UnprocessableEntity();
		}
	}
}
