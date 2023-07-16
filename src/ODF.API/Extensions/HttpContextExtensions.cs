using System.Security.Claims;
using ODF.Domain;
using ODF.Domain.Constants;

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

		public static string GetUserName(this HttpContext context)
		{
			string? name = context.User?.FindFirstValue(ClaimTypes.Name);

			if (!string.IsNullOrEmpty(name))
			{
				return name;
			}

			string? email = context.User?.FindFirstValue(ClaimTypes.Email);

			if (!string.IsNullOrEmpty(email))
			{
				return email;
			}

			return string.Empty;
		}

		public static bool IsLoggedIn(this HttpContext context)
			=> context.User.Identity?.IsAuthenticated ?? false;

		public static string GetCountryCodeFromLang(this HttpContext httpContext)
		{
			string userLang = httpContext.Request.GetTypedHeaders().AcceptLanguage.FirstOrDefault()?.Value.Value ?? "";
			return Languages.TryParse(userLang, out var lang) ? lang!.GetCountryCode() : Languages.Czech.GetCountryCode();
		}

		public static bool IsApiRequest(this HttpContext httpContext)
			=> !(httpContext.Request.Path == new PathString("/") ||
				httpContext.Request.Path == new PathString("/metrics") ||
				httpContext.Request.Path.ToString().Contains("swagger") ||
				httpContext.Request.Path.ToString().Contains("signal") ||
				httpContext.Request.Path.ToString().Contains("health")
			);
	}
}
