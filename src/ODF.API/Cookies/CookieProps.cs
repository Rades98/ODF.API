using Microsoft.AspNetCore.Authentication;

namespace ODF.API.Cookies
{
	public static class CookieProps
	{
		public static AuthenticationProperties AuthProps => _authProps;
		public static CookieOptions BaseCookieOpts => _baseCookieOpts;

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
			SameSite = SameSiteMode.Strict,
			Expires = DateTime.UtcNow.AddDays(4)
		};
	}
}
