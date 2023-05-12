using MediatR;

namespace ODF.API.Middleware
{
	public class LoggingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<LoggingMiddleware> _logger;

		public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
		{
			_next = next;
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task Invoke(HttpContext httpContext)
		{
			_logger.LogInformation("{endpoint} cookies: {cookies}", httpContext.Request.Path, httpContext.Request.Cookies);

			await _next(httpContext);
		}
	}
}
