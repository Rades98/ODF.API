using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Nest;
using ODF.Data.Contracts.Entities;
using ODF.Data.Contracts.Interfaces;

namespace ODF.Data.Elastic.Repos.Lineups
{
	internal class LineupRepo : ILineupRepo
	{
		private readonly IElasticClient _elasticClient;

		public LineupRepo(IElasticClient elasticClient)
		{
			_elasticClient = elasticClient ?? throw new ArgumentNullException(nameof(elasticClient));
		}


		public async Task<bool> AddLineupItemAsync(LineupItem lineupItem, CancellationToken cancellationToken)
		{
			var res = await _elasticClient.IndexAsync(lineupItem, i => i, cancellationToken);

			return res.IsValid;
		}

		public async Task<IEnumerable<LineupItem>> GetLineupAsync(CancellationToken cancellationToken)
			=> (await _elasticClient.SearchAsync<LineupItem>()).Documents;
	}
}
