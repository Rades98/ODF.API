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
			=> (await _elasticClient.IndexAsync(lineupItem, i => i, cancellationToken)).IsValid;

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

			lineupItem.PerformanceName.AddToScriptWithParamIsfEdited(nameof(lineupItem.PerformanceName), scriptParams, script);
			lineupItem.UserName.AddToScriptWithParamIsfEdited(lineupItem.UserName, scriptParams, script);
			lineupItem.Interpret.AddToScriptWithParamIsfEdited(nameof(lineupItem.Interpret), scriptParams, script);
			lineupItem.Place.AddToScriptWithParamIsfEdited(nameof(lineupItem.Place), scriptParams, script);
			lineupItem.DateTime.AddToScriptWithParamIsfEdited(nameof(lineupItem.DateTime), scriptParams, script);

			return (await _elasticClient.UpdateByQueryAsync<LineupItem>(s => s
					.Query(q => q
						.Bool(bq => bq
							.Filter(
								fq => fq.Terms(t => t.Field(f => f.Id).Terms(lineupItem.Id))
							)
						)
					)
					.Size(1)
					.Script(s => s
						.Source(script.ToString())
						.Params(scriptParams))
					.Refresh(true), cancellationToken)).IsValid;
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
