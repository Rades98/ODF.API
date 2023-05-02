using System.Net;
using MediatR;
using ODF.API.Extensions;
using ODF.API.ResponseModels.Exceptions;
using ODF.AppLayer.CQRS.Translations.Queries;
using ODF.AppLayer.Pipelines;

namespace ODF.API.Middleware
{
	public class ExceptionHandlingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly IMediator _mediator;

		public ExceptionHandlingMiddleware(RequestDelegate next, IMediator mediator)
		{
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
			_next = next;
		}

		public async Task Invoke(HttpContext httpContext)
		{
			string? countryCode = httpContext.GetCountryCode();

			try
			{
				await _next(httpContext);
			}
			catch (Exception e)
			{
				string resultMsg;

				if (e is TranslationNotFoundException)
				{
					resultMsg = await _mediator.Send(new GetTranslationQuery("Nepodporovaný jazyk", "server_error_wrong_language", countryCode ?? "CZ"), default);
				}
				else
				{
					resultMsg = await _mediator.Send(new GetTranslationQuery("Ooops, něco se nepovedlo", "internal_server_error", countryCode ?? "CZ"), default);
				}

				 
				var responseModel = new ExceptionResponseModel(resultMsg);

				httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
				await httpContext.Response.WriteAsync(responseModel.ToString());
			}
		}
	}
}
