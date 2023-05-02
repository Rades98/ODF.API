﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
				var loginTranslation = await mediator.Send(new GetTranslationQuery("Uživatelské jméno", "login_username", countryCode), default);
				var passwordTranslation = await mediator.Send(new GetTranslationQuery("Heslo", "login_pw", countryCode), default);

				// MOCK
				if (user.UserName == "admin" && user.Password == "heslopyco")
				{
					var userResult = await mediator.Send(new LoginUserCommand("", ""), cancellationToken);

					var key = Encoding.ASCII.GetBytes(conf["Jwt:Key"]!);
					var tokenDescriptor = new SecurityTokenDescriptor
					{
						Subject = new ClaimsIdentity(new[]
						{
							new Claim(JwtRegisteredClaimNames.Sub, userResult.UserName),
							new Claim(JwtRegisteredClaimNames.Email, userResult.Email),
							new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
						}),
						Expires = DateTime.UtcNow.AddHours(2),
						SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
					};

					tokenDescriptor.Subject.AddClaims(userResult.Claims);

					var tokenHandler = new JwtSecurityTokenHandler();
					var token = tokenHandler.CreateToken(tokenDescriptor);
					var stringToken = tokenHandler.WriteToken(token);

					context.Response.Cookies.Append("Token", stringToken);

					var responseModel = new UserResponseModel(apiSettings.ApiUrl, userResult.UserName, stringToken, countryCode, UserFormFactory.GetLoginForm(loginTranslation, passwordTranslation));

					return Results.Ok(responseModel);
				}

				var password2Translation = await mediator.Send(new GetTranslationQuery("Heslo pro kontrolu", "login_pw2", countryCode), default);
				var emailTranslation = await mediator.Send(new GetTranslationQuery("Heslo pro kontrolu", "login_email", countryCode), default);
				var firstNameTranslation = await mediator.Send(new GetTranslationQuery("Heslo pro kontrolu", "login_first_name", countryCode), default);
				var lastNameTranslation = await mediator.Send(new GetTranslationQuery("Heslo pro kontrolu", "login_last_name", countryCode), default);

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

			return app;
		}
	}
}
