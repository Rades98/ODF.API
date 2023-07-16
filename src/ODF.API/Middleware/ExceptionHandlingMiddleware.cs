using System.Net;
using ODF.API.Extensions;
using ODF.API.ResponseModels.Exceptions;
using ODF.AppLayer.Extensions;
using ODF.AppLayer.Services.Interfaces;
using ODF.Domain;

namespace ODF.API.Middleware
{
	public class ExceptionHandlingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ITranslationsProvider _translationsProvider;
		private readonly ILogger _logger;

		public ExceptionHandlingMiddleware(RequestDelegate next, ITranslationsProvider translationsProvider, ILogger<ExceptionHandlingMiddleware> logger)
		{
			_translationsProvider = translationsProvider ?? throw new ArgumentNullException(nameof(translationsProvider));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_next = next;
		}

		public async Task Invoke(HttpContext httpContext)
		{
			string countryCode = httpContext.GetCountryCodeFromLang() ?? Languages.Czech.GetCountryCode();
			var translations = await _translationsProvider.GetTranslationsAsync(countryCode, default);

			try
			{
				await _next(httpContext);
			}
			catch (Exception e)
			{
				_logger.LogError("Exception has occured {message} with inner message {inner_exception}", e.Message, e.InnerException);
				string resultMsg = translations.Get("internal_server_error") ?? "Internal server error";

				var responseModel = new ExceptionResponseModel(resultMsg);

				httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
				await httpContext.Response.WriteAsync(responseModel.ToString());
			}
		}
	}
}
