using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Nest;
using ODF.AppLayer.Repos;
using ODF.Domain.Entities;

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

		public async Task<bool> RemoveLineupItemAsync(Guid id, CancellationToken cancellationToken)
			=> (await _elasticClient.DeleteByQueryAsync<LineupItem>(q => q
					.Query(rq => rq
						.Match(m => m
						.Field(f => f.Id)
						.Query(id.ToString()))
					), cancellationToken
				)).IsValid;

		public async Task<bool> UpdateLineupItemAsync(LineupItem lineupItem, CancellationToken cancellationToken)
		{
			var scriptParams = new Dictionary<string, object>();
			StringBuilder script = new StringBuilder();

			if (!string.IsNullOrEmpty(lineupItem.PerformanceName))
			{
				scriptParams.Add(nameof(LineupItem.PerformanceName), lineupItem.PerformanceName);
				script.Append($"ctx._source.performanceName = params.{nameof(LineupItem.PerformanceName)};");
			}

			if (!string.IsNullOrEmpty(lineupItem.UserName))
			{
				scriptParams.Add(nameof(LineupItem.UserName), lineupItem.UserName);
				script.Append($"ctx._source.userName = params.{nameof(LineupItem.UserName)};");
			}

			if (!string.IsNullOrEmpty(lineupItem.Interpret))
			{
				scriptParams.Add(nameof(LineupItem.Interpret), lineupItem.Interpret);
				script.Append($"ctx._source.interpret = params.{nameof(LineupItem.Interpret)};");
			}

			if (!string.IsNullOrEmpty(lineupItem.Place))
			{
				scriptParams.Add(nameof(LineupItem.Place), lineupItem.Place);
				script.Append($"ctx._source.place = params.{nameof(LineupItem.Place)};");
			}

			scriptParams.Add(nameof(LineupItem.DateTime), lineupItem.DateTime.ToString("yyyy-MM-ddTHH:mm:ss"));
			script.Append($"ctx._source.dateTime = params.{nameof(LineupItem.DateTime)};");


			var res = await _elasticClient.UpdateByQueryAsync<LineupItem>(s => s
					.MatchAll()
					.Script(s => s
						.Source(script.ToString())
						.Params(scriptParams))
					.Refresh(true), cancellationToken);

			return res.IsValid;
		}

		public async Task<LineupItem> GetAsync(Guid id, CancellationToken cancellationToken)
			=> (await _elasticClient.SearchAsync<LineupItem>(s => s
							.Query(q => q
								.Bool(bq => bq
									.Filter(
										fq => fq.Terms(t => t.Field(f => f.Id).Terms(id))
									)
								)
							)
							.Size(1), cancellationToken)).Documents.FirstOrDefault();
	}
}
