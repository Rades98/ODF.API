using ODF.AppLayer.Consts;
using ODF.Domain;
using System.Security.Claims;

namespace ODF.API.Extensions
{
	public static class HttpContextExtensions
	{
		private const string CountryParam = "countryCode";

		public static bool IsAdmin(this HttpContext context)
			=> context.User.FindFirstValue(ClaimTypes.Role) == UserRoles.Admin;

		public static Guid? GetUserId(this HttpContext context)
		{
			string id = context.User.FindFirstValue(ClaimTypes.Actor);
			if (!string.IsNullOrEmpty(id))
			{
				return Guid.Parse(id);
			}

			return null;
		}

		public static bool IsLoggedIn(this HttpContext context)
			=> context.User.Identity?.IsAuthenticated ?? false;

		public static string? GetCountryCode(this HttpContext httpContext)
		{
			if (!httpContext.IsApiRequest())
			{
				return Languages.Czech.GetCountryCode().ToLower();
			}

			if (httpContext.Request.Query.ContainsKey(CountryParam))
			{
				return httpContext.Request.Query[CountryParam];
			}
			else if (httpContext.Request.Headers.ContainsKey(CountryParam))
			{
				return httpContext.Request.Headers[CountryParam];
			}
			else if (httpContext.Request.RouteValues.ContainsKey(CountryParam))
			{
				return httpContext.Request.RouteValues[CountryParam]!.ToString();
			}

			return null;
		}

		public static bool IsApiRequest(this HttpContext httpContext)
			=> !(httpContext.Request.Path == new PathString("/") ||
				httpContext.Request.Path == new PathString("/metrics") ||
				httpContext.Request.Path.ToString().Contains(".ico") ||
				httpContext.Request.Path.ToString().Contains("ui") ||
				httpContext.Request.Path.ToString().Contains("health")
			);
	}
}
