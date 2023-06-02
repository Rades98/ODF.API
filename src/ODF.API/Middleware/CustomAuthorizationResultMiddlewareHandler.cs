using System.Text;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using ODF.API.Extensions;
using ODF.API.FormFactories;
using ODF.API.ResponseModels.Common;
using ODF.API.ResponseModels.Exceptions;
using ODF.AppLayer.CQRS.Translations.Queries;

namespace ODF.API.Middleware
{
	public class CustomAuthorizationResultMiddlewareHandler : IAuthorizationMiddlewareResultHandler
	{
		private readonly IMediator _mediator;
		private readonly AuthorizationMiddlewareResultHandler DefaultHandler = new();

		public CustomAuthorizationResultMiddlewareHandler(IMediator mediator)
		{
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
		}

		public async Task HandleAsync(RequestDelegate next, HttpContext httpContext, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
		{
			if (!authorizeResult.Succeeded)
			{
				string? countryCode = httpContext.GetCountryCode();

				string title = await _mediator.Send(new GetTranslationQuery("Neoprávněná akce", "unauthorized_title", countryCode), default);

				string message;

				bool isLogged = httpContext!.IsLoggedIn();

				if (isLogged)
				{
					message = await _mediator.Send(new GetTranslationQuery("Pro tuto akci nemáte dostatečné oprávnění. Pokud se domníváte, že je máte mít, obraťte se na administrátora.", "unauthorized_msg_logged", countryCode), default);
				}
				else
				{
					message = await _mediator.Send(new GetTranslationQuery("Pro vykonání této akce je třeba se přihlásit.", "unauthorized_msg_annonymous", countryCode), default);
				}

				string loginActionName = await _mediator.Send(new GetTranslationQuery("Přihlásit se", "login", countryCode), default);
				string loginTranslation = await _mediator.Send(new GetTranslationQuery("Uživatelské jméno", "login_username", countryCode), default);
				string passwordTranslation = await _mediator.Send(new GetTranslationQuery("Heslo", "login_pw", countryCode), default);

				string link = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/{countryCode}/user";
				var loginAction = new NamedAction(link, loginActionName, "login", HttpMethods.Post, UserFormFactory.GetLoginForm(loginTranslation, passwordTranslation));

				var responseModel = new UnauthorizedExceptionResponseModel(title, message, null, !isLogged ? loginAction : null);

				byte[] bytes = Encoding.UTF8.GetBytes(responseModel.ToString());
				httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
				httpContext.Response.ContentType = "application/json";
				await httpContext.Response.Body.WriteAsync(bytes, 0, bytes.Length);
			}
			else
			{
				await DefaultHandler.HandleAsync(next, httpContext, policy, authorizeResult);
			}
		}
	}
}
