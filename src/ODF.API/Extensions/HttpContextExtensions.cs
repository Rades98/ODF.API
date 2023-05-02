﻿using System.Security.Claims;
using ODF.AppLayer.Consts;
using ODF.Enums;

namespace ODF.API.Extensions
{
	public static class HttpContextExtensions
	{
		public static bool IsAdmin(this HttpContext context)
			=> context.User.FindFirstValue(ClaimTypes.Role) == UserRoles.Admin;

		public static bool IsLoggedIn(this HttpContext context)
			=> context.User.Identity?.IsAuthenticated ?? false;

		public static string? GetCountryCode(this HttpContext httpContext)
		{
			string CountryParam = "countryCode";

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
			else if (httpContext.Request.Path == new PathString("/"))
			{
				return Languages.Czech.GetCountryCode().ToLower();
			}

			return null;
		}
	}
}
