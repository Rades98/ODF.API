using Microsoft.Extensions.Diagnostics.HealthChecks;
using Nest;

namespace ODF.API.HealthChecks
{
	internal class ElasticHealthCheck : IHealthCheck
	{
		private readonly IElasticClient _elasticClient;

		public ElasticHealthCheck(IElasticClient elasticClient)
		{
			_elasticClient = elasticClient ?? throw new ArgumentNullException(nameof(elasticClient));
		}

		/// <inheritdoc/>
		public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
		{

			if (_elasticClient.Cluster.Health().IsValid)
			{
				return Task.FromResult(HealthCheckResult.Healthy("Elastic is healthy"));
			}

			return Task.FromResult(HealthCheckResult.Unhealthy("Elastic is not healthy"));
		}
	}
}
