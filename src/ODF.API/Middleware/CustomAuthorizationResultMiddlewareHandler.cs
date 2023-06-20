using System.Net.Mime;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using ODF.API.Controllers.Users;
using ODF.API.Extensions;
using ODF.API.FormFactories;
using ODF.API.ResponseModels.Exceptions;
using ODF.AppLayer.Extensions;
using ODF.AppLayer.Services.Interfaces;
using ODF.Domain;

namespace ODF.API.Middleware
{
    public class CustomAuthorizationResultMiddlewareHandler : IAuthorizationMiddlewareResultHandler
	{
		private readonly AuthorizationMiddlewareResultHandler DefaultHandler = new();
		private readonly ITranslationsProvider _translationsProvider;
		private readonly IActionDescriptorCollectionProvider _adcp;

		public CustomAuthorizationResultMiddlewareHandler(ITranslationsProvider translationsProvider, IActionDescriptorCollectionProvider adcp)
		{
			_translationsProvider = translationsProvider ?? throw new ArgumentNullException(nameof(translationsProvider));
			_adcp = adcp ?? throw new ArgumentNullException(nameof(adcp));
		}

		public async Task HandleAsync(RequestDelegate next, HttpContext httpContext, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
		{
			if (!authorizeResult.Succeeded)
			{
				string? countryCode = httpContext.GetCountryCode() ?? Languages.Czech.GetCountryCode();

				var translations = await _translationsProvider.GetTranslationsAsync(countryCode, default);

				string title = translations.Get("unauthorized_title");

				bool isLoggedIn = httpContext!.IsLoggedIn();

				string message = isLoggedIn ? translations.Get("unauthorized_msg_logged") : translations.Get("unauthorized_msg_annonymous");

				var loginAction = _adcp.GetNamedAction(httpContext, $"{httpContext.Request.Scheme}://{httpContext.Request.Host}",
					nameof(UsersController.LoginUser), translations.Get("login_user"), "login", UserFormFactory.GetLoginForm(translations));

				var responseModel = new UnauthorizedExceptionResponseModel(title, message, null, !isLoggedIn ? loginAction : null);

				byte[] bytes = Encoding.UTF8.GetBytes(responseModel.ToString());
				httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
				httpContext.Response.ContentType = MediaTypeNames.Application.Json;
				await httpContext.Response.Body.WriteAsync(bytes, 0, bytes.Length);
			}
			else
			{
				await DefaultHandler.HandleAsync(next, httpContext, policy, authorizeResult);
			}
		}
	}
}
