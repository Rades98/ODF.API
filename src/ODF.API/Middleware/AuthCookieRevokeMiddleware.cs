using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using ODF.API.Cookies;

namespace ODF.API.Middleware
{
	public class AuthCookieRevokeMiddleware
	{
		private readonly RequestDelegate _next;

		public AuthCookieRevokeMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext httpContext)
		{
			var auth = httpContext.RequestServices.GetRequiredService<IAuthenticationService>().AuthenticateAsync(httpContext, CookieAuthenticationDefaults.AuthenticationScheme);
			var expiration = auth.Result?.Properties?.ExpiresUtc;

			if (expiration is not null)
			{
				if (expiration.Value < DateTimeOffset.Now)
				{
					await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
				}
				else if (expiration.Value < DateTimeOffset.Now.AddHours(8))
				{
					var claimsIdentity = new ClaimsIdentity(auth.Result?.Principal?.Claims, CookieAuthenticationDefaults.AuthenticationScheme);
					await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), CookieProps.AuthProps);
				}
			}

			await _next(httpContext);
		}
	}
}
