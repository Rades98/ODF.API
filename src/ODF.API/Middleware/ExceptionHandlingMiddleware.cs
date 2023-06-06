using System.Net;
using FluentValidation;
using ODF.API.Extensions;
using ODF.API.ResponseModels.Exceptions;
using ODF.AppLayer.Extensions;
using ODF.AppLayer.Pipelines;
using ODF.AppLayer.Services.Interfaces;

namespace ODF.API.Middleware
{
	public class ExceptionHandlingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ITranslationsProvider _translationsProvider;

		public ExceptionHandlingMiddleware(RequestDelegate next, ITranslationsProvider translationsProvider)
		{
			_translationsProvider = translationsProvider ?? throw new ArgumentNullException(nameof(translationsProvider));
			_next = next;
		}

		public async Task Invoke(HttpContext httpContext)
		{
			string countryCode = httpContext.GetCountryCode() ?? "CZ";
			var translations = await _translationsProvider.GetTranslationsAsync(countryCode, default);

			try
			{
				await _next(httpContext);
			}
			catch (Exception e)
			{
				string resultMsg;

				if (e is TranslationNotFoundException)
				{
					resultMsg = translations.Get("server_error_wrong_language");
				}
				else if (e is ValidationException validationException)
				{
					resultMsg = string.Join(",", validationException.Errors);
				}
				else
				{
					resultMsg = translations.Get("internal_server_error");
				}


				var responseModel = new ExceptionResponseModel(resultMsg);

				httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
				await httpContext.Response.WriteAsync(responseModel.ToString());
			}
		}
	}
}
