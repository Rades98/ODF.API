using System.Net.Mime;
using System.Text;
using Microsoft.AspNetCore.Mvc.Filters;
using ODF.API.Extensions;
using ODF.AppLayer.Extensions;
using ODF.AppLayer.Services.Interfaces;
using ODF.Domain;

namespace ODF.API.Attributes.HtttpMethodAttributes
{
	public class CountryCodeFilterAttribute : ActionFilterAttribute
	{
		private readonly string _countryCode;

		public CountryCodeFilterAttribute(string countryCode)
		{
			_countryCode = Languages.TryParse(countryCode, out _) ? countryCode : Languages.English.GetCountryCode();
		}

		public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			string? countryCode = context.HttpContext.GetCountryCodeFromLang();

			if (countryCode != _countryCode)
			{
				context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
				var translationsProvider = context.HttpContext.RequestServices.GetService<ITranslationsProvider>();

				if (translationsProvider is not null)
				{
					var translations = await translationsProvider.GetTranslationsAsync(countryCode, default);

					byte[] bytes = Encoding.UTF8.GetBytes(string.Format(translations.Get("supported_lang_only"), _countryCode));
					context.HttpContext.Response.ContentType = MediaTypeNames.Application.Json;
					await context.HttpContext.Response.Body.WriteAsync(bytes);
				}
			}
			else
			{
				await next();
			}
		}
	}
}
