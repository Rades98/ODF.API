using Microsoft.Extensions.Diagnostics.HealthChecks;
using StackExchange.Redis;

namespace ODF.API.HealthChecks
{
	internal class RedisHealthCheck : IHealthCheck
	{
		private readonly IConnectionMultiplexer _connectionMultiplexer;

		public RedisHealthCheck(IConnectionMultiplexer connectionMultiplexer)
		{
			_connectionMultiplexer = connectionMultiplexer ?? throw new ArgumentNullException(nameof(connectionMultiplexer));
		}

		public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
		{
			if (_connectionMultiplexer.IsConnected)
			{
				return Task.FromResult(HealthCheckResult.Healthy("Redis is healthy"));
			}

			if (_connectionMultiplexer.IsConnecting)
			{
				return Task.FromResult(HealthCheckResult.Degraded("Redis connecting"));
			}

			return Task.FromResult(HealthCheckResult.Unhealthy("Redis is inaccessible"));
		}
	}
}
