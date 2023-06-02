using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ODF.API.Controllers.Base;
using ODF.API.Extensions;
using ODF.API.FormFactories;
using ODF.API.Registration.SettingModels;
using ODF.API.RequestModels.Forms;
using ODF.API.ResponseModels.Common;
using ODF.API.ResponseModels.Exceptions;
using ODF.API.ResponseModels.User;
using ODF.API.Responses;
using ODF.AppLayer.CQRS.Translations.Queries;
using ODF.AppLayer.CQRS.User.Commands;

namespace ODF.API.Controllers
{
	public class UsersController : BaseController
	{
		public UsersController(IMediator mediator, IOptions<ApiSettings> apiSettings) : base(mediator, apiSettings)
		{
		}

		[HttpPost("/{countryCode}/user/login")]
		[ProducesResponseType(typeof(UserResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(UnauthorizedExceptionResponseModel), StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> Login([FromBody] UserRequestForm user, [FromRoute] string countryCode, CancellationToken cancellationToken)
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

			string loginTranslation = await Mediator.Send(new GetTranslationQuery("Uživatelské jméno", "login_username", countryCode), cancellationToken);
			string passwordTranslation = await Mediator.Send(new GetTranslationQuery("Heslo", "login_pw", countryCode), cancellationToken);

			//Tohle je treba doresit protoze UserValidationDto se nechytne v ValidationDto based pipeline
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

				var responseModel = new UserResponseModel(ApiSettings.ApiUrl, userResult.User.UserName, countryCode, UserFormFactory.GetLoginForm(loginTranslation, passwordTranslation));
				responseModel.AddAction($"/{countryCode}/navigation", "nav", HttpMethods.Get);

				return Ok(responseModel);
			}

			string loginActionName = await Mediator.Send(new GetTranslationQuery("Přihlásit se", "login", countryCode), cancellationToken);
			string link = $"{ApiSettings.ApiUrl}/{countryCode}/user";

			var loginAction = new NamedAction(link, loginActionName, "login", HttpMethods.Post, UserFormFactory.GetLoginForm(loginTranslation, passwordTranslation, errors: userResult.Errors));

			string password2Translation = await Mediator.Send(new GetTranslationQuery("Heslo pro kontrolu", "login_pw2", countryCode), cancellationToken);
			string emailTranslation = await Mediator.Send(new GetTranslationQuery("e-mail", "login_email", countryCode), cancellationToken);
			string firstNameTranslation = await Mediator.Send(new GetTranslationQuery("Jméno", "login_first_name", countryCode), cancellationToken);
			string lastNameTranslation = await Mediator.Send(new GetTranslationQuery("Příjmení", "login_last_name", countryCode), cancellationToken);

			string title = await Mediator.Send(new GetTranslationQuery("Přihlášení se nezdařilo", "login_failed_title", countryCode), cancellationToken);
			string message = await Mediator.Send(new GetTranslationQuery("Zkontrolujte, že jste zadali správné údaje k účtu", "login_failed_msg", countryCode), cancellationToken);

			string registrationActionName = await Mediator.Send(new GetTranslationQuery("Nemáte registraci? Klikněte zde!", "register_action_name", countryCode), cancellationToken);

			var registerAction = new NamedAction(ApiSettings.ApiUrl + $"/{countryCode}/user", registrationActionName, "register", HttpMethods.Put,
				UserFormFactory.GetRegisterForm(loginTranslation, passwordTranslation, password2Translation, emailTranslation, firstNameTranslation, lastNameTranslation));

			return CustomApiResponses.Unauthorized(new UnauthorizedExceptionResponseModel(title, message, loginAction, registerAction));
		}

		[HttpPut("/{countryCode}/user/register")]
		public IActionResult RegisterUser([FromRoute] string countryCode)
		{
			return Ok();
		}

		[HttpPost("/{countryCode}/user/logout")]
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
