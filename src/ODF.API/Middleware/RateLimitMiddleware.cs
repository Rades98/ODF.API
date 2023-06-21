using System.Net;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using ODF.API.Extensions;
using ODF.API.Registration.SettingModels;

namespace ODF.API.Middleware
{
	public class RateLimitMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly IDistributedCache _cache;
		private readonly ILogger<RateLimitMiddleware> _logger;
		private readonly AntiScrappingSettings _scrapOpts;

		public RateLimitMiddleware(RequestDelegate next, IDistributedCache cache, ILogger<RateLimitMiddleware> logger, IOptions<AntiScrappingSettings> scrapOpts)
		{
			_next = next;
			_cache = cache ?? throw new ArgumentNullException(nameof(cache));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_scrapOpts = scrapOpts.Value ?? throw new ArgumentNullException(nameof(scrapOpts));
		}

		public async Task Invoke(HttpContext httpContext)
		{
			CancellationToken cancellationToken = default;

			string key = $"{httpContext.Request.Path}_{httpContext.Connection.RemoteIpAddress}";
			var clientStatistics = await _cache.GetCachedValueAsyn<ClientStatistics>(key, cancellationToken) ?? new() { LastSuccessfulResponseTime = DateTime.Now };

			var blockedIp = await _cache.GetCachedValueAsyn<BlockedIp>($"block_{httpContext.Connection.RemoteIpAddress}", cancellationToken);

			if (blockedIp is not null && blockedIp.AllertCount >= _scrapOpts.WarningCount)
			{
				_logger.LogWarning("Some boii has been banned for scrapping from {ip}", httpContext.Connection.RemoteIpAddress);
				httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
				await httpContext.Response.WriteAsync($"U were warned.. now u r banned for cca {_scrapOpts.DurationMin} mins (͡ ° ͜ʖ ͡ °) FUCK OFF DUDE!!", cancellationToken);

				return;
			}

			if (DateTime.Now < clientStatistics.LastSuccessfulResponseTime.AddSeconds(2) &&
				clientStatistics.NumberofRequestsCompletedSuccessfully >= _scrapOpts.MaxCallsPerPage)
			{
				_logger.LogWarning("Some boii is scrapping {path} from {ip}", httpContext.Request.Path, httpContext.Connection.RemoteIpAddress);

				httpContext.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
				await httpContext.Response.WriteAsync($"Stop scrapping me, please, it hurts. :(", cancellationToken);

				await UpdateBlockingAsync(httpContext.Connection.RemoteIpAddress!.ToString(), cancellationToken);

				return;
			}

			await UpdateClientStatisticsAsync(key, cancellationToken);
			await _next(httpContext);
		}

		private async Task UpdateClientStatisticsAsync(string key, CancellationToken cancellationToken)
		{
			var clientStats = _cache.GetCachedValueAsyn<ClientStatistics>(key, cancellationToken).Result;
			if (clientStats is not null)
			{
				clientStats.LastSuccessfulResponseTime = DateTime.Now;
				if (clientStats.NumberofRequestsCompletedSuccessfully >= _scrapOpts.MaxCallsPerPage)
				{
					clientStats.NumberofRequestsCompletedSuccessfully = 1;
				}
				else
				{
					clientStats.NumberofRequestsCompletedSuccessfully++;
				}
			}
			else
			{
				clientStats = new ClientStatistics
				{
					LastSuccessfulResponseTime = DateTime.Now,
					NumberofRequestsCompletedSuccessfully = 1
				};
			}

			await _cache.SetCachedValueAsync(key, clientStats, cancellationToken: cancellationToken);
		}

		private async Task UpdateBlockingAsync(string key, CancellationToken cancellationToken)
		{
			var blockedIp = await _cache.GetCachedValueAsyn<BlockedIp>($"block_{key}", cancellationToken);

			if (blockedIp is not null)
			{
				blockedIp.AllertCount += 1;
			}
			else
			{
				blockedIp = new()
				{
					Ip = key,
					AllertCount = 1
				};
			}

			await _cache.SetCachedValueAsync($"block_{key}", blockedIp, new() { AbsoluteExpiration = DateTime.Now.AddMinutes(_scrapOpts.DurationMin) }, cancellationToken);
		}

		private sealed record ClientStatistics()
		{
			public DateTime LastSuccessfulResponseTime { get; set; }
			public int NumberofRequestsCompletedSuccessfully { get; set; }
		}

		private sealed record BlockedIp()
		{
			public string Ip { get; set; } = string.Empty;

			public int AllertCount { get; set; }
		}
	}
}
