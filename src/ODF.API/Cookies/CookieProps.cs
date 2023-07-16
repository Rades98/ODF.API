using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace ODF.API.Cookies
{
	public static class CookieProps
	{
		public static AuthenticationProperties AuthProps => _authProps;
		public static CookieOptions BaseCookieOpts => _baseCookieOpts;

		public static Action<CookieAuthenticationOptions> CookieAuthenticationOpts => _cookieAuthenticationOptions;

		private static AuthenticationProperties _authProps = new()
		{
			AllowRefresh = true,
			ExpiresUtc = DateTimeOffset.UtcNow.AddDays(2),
			IsPersistent = true,
		};

		private static readonly CookieOptions _baseCookieOpts = new()
		{
			HttpOnly = false,
			IsEssential = true,
			Secure = false,
			Expires = DateTime.UtcNow.AddDays(4),
			SameSite = SameSiteMode.None,
		};

		private static Action<CookieAuthenticationOptions> _cookieAuthenticationOptions => opts =>
		{
			opts.ExpireTimeSpan = TimeSpan.FromDays(2);
			opts.SlidingExpiration = true;
			opts.Cookie.Name = "folklorova-auth_cookie";
			opts.Cookie.SameSite = SameSiteMode.None;
			opts.Cookie.SecurePolicy = CookieSecurePolicy.Always;
		};
	}
}
