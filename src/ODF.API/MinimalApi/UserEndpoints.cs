using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Nest;
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

namespace ODF.API.MinimalApi
{
	public static class UserEndpoints
	{
		public static WebApplication MapUserEndpoints(this WebApplication app, IMediator mediator, ApiSettings apiSettings)
		{
			//login
			app.MapPost("/{countryCode}/user", async ([FromBody] UserRequestForm user, [FromRoute] string countryCode, HttpContext context, IConfiguration conf, CancellationToken cancellationToken) =>
			{
				await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
				var loginTranslation = await mediator.Send(new GetTranslationQuery("Uživatelské jméno", "login_username", countryCode), default);
				var passwordTranslation = await mediator.Send(new GetTranslationQuery("Heslo", "login_pw", countryCode), default);

				// MOCK
				if (user.UserName == "admin" && user.Password == "heslopyco")
				{
					var userResult = await mediator.Send(new LoginUserCommand("admin", "adminPW"), cancellationToken); //work with mock

					var claimsIdentity = new ClaimsIdentity(userResult.Claims, CookieAuthenticationDefaults.AuthenticationScheme);

					var authProperties = new AuthenticationProperties
					{
						AllowRefresh = true,
						ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1),
						IsPersistent = true,
					};

					await context.SignInAsync(
						CookieAuthenticationDefaults.AuthenticationScheme,
						new ClaimsPrincipal(claimsIdentity),
						authProperties);

					var responseModel = new UserResponseModel(apiSettings.ApiUrl, userResult.UserName, countryCode, UserFormFactory.GetLoginForm(loginTranslation, passwordTranslation));
					responseModel.AddAction($"/{countryCode}/navigation", "nav", HttpMethods.Get);

					return Results.Ok(responseModel);
				}

				var password2Translation = await mediator.Send(new GetTranslationQuery("Heslo pro kontrolu", "login_pw2", countryCode), default);
				var emailTranslation = await mediator.Send(new GetTranslationQuery("e-mail", "login_email", countryCode), default);
				var firstNameTranslation = await mediator.Send(new GetTranslationQuery("Jméno", "login_first_name", countryCode), default);
				var lastNameTranslation = await mediator.Send(new GetTranslationQuery("Příjmení", "login_last_name", countryCode), default);

				var title = await mediator.Send(new GetTranslationQuery("Přihlášení se nezdařilo", "login_failed_title", countryCode), default);
				string message = await mediator.Send(new GetTranslationQuery("Zkontrolujte, že jste zadali správné údaje k účtu", "login_failed_msg", countryCode), default);

				string registrationActionName = await mediator.Send(new GetTranslationQuery("Nemáte registraci? Klikněte zde!", "register_action_name", countryCode), default);

				var registerAction = new NamedAction(apiSettings.ApiUrl + $"/{countryCode}/user", registrationActionName, "register", HttpMethods.Put, 
					UserFormFactory.GetRegisterForm(loginTranslation, passwordTranslation, password2Translation, emailTranslation, firstNameTranslation, lastNameTranslation));

				return CustomApiResponses.Unauthorized(new(title, message, registerAction));
			})
			.WithMetadata(new ProducesResponseTypeAttribute(typeof(UserResponseModel), StatusCodes.Status200OK))
			.WithMetadata(new ProducesResponseTypeAttribute(typeof(UnauthorizedExceptionResponseModel), StatusCodes.Status401Unauthorized));

			//register
			app.MapPut("/{countryCode}/user", (HttpContext context) =>
			{
			});

			app.MapPost("/{countryCode}/user/logout", async ([FromRoute] string countryCode, HttpContext context, IConfiguration conf, CancellationToken cancellationToken) =>
			{
				if(context.IsLoggedIn())
				{
					await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
					return Results.Accepted();
				}

				return Results.UnprocessableEntity();
			});

			return app;
		}
	}
}
