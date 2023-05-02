using System.Net;
using ODF.API.Extensions;
using ODF.Enums;

namespace ODF.API.Middleware
{
	public class CountryCodeMiddleWare
	{
		private readonly RequestDelegate _next;

		public CountryCodeMiddleWare(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext httpContext)
		{
			if (!Languages.TryParse(httpContext.GetCountryCode(), out var lang))
			{
				httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest; 
				return;
			}

			await _next(httpContext);
		}
	}
}
