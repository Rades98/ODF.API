using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using ODF.AppLayer.Repos;
using ODF.Domain.Entities;

namespace ODF.Data.Elastic.Repos.Lineups
{
	internal class LineupRepoCahce : ILineupRepo
	{
		private readonly IMemoryCache _cache;
		private readonly ILineupRepo _repo;

		public LineupRepoCahce(IMemoryCache cache, ILineupRepo repo)
		{
			_cache = cache ?? throw new ArgumentNullException(nameof(cache));
			_repo = repo ?? throw new ArgumentNullException(nameof(repo));
		}

		public Task<bool> AddLineupItemAsync(LineupItem lineupItem, CancellationToken cancellationToken)
			=> _repo.AddLineupItemAsync(lineupItem, cancellationToken);

		public async Task<IEnumerable<LineupItem>> GetLineupAsync(CancellationToken cancellationToken)
		{
			string cacheKey = $"{nameof(LineupItem)}s";

			object cachedResponse = _cache.Get(cacheKey);

			if (cachedResponse != null)
			{
				var res = JsonConvert.DeserializeObject<IEnumerable<LineupItem>>(Encoding.Default.GetString((byte[])cachedResponse));
				if (res is not null)
				{
					return res;
				}
			}

			var response = await _repo.GetLineupAsync(cancellationToken);

			_cache.Set(cacheKey, Encoding.Default.GetBytes(JsonConvert.SerializeObject(response)), new MemoryCacheEntryOptions()
			{
				AbsoluteExpiration = DateTime.Now.AddMinutes(60)
			});

			return response;
		}

		public Task<bool> RemoveLineupItemAsync(Guid id, CancellationToken cancellationToken)
			=> _repo.RemoveLineupItemAsync(id, cancellationToken);

		public Task<bool> UpdateLineupItemAsync(LineupItem lineupItem, CancellationToken cancellationToken)
			=> UpdateLineupItemAsync(lineupItem, cancellationToken);

		public Task<LineupItem> GetAsync(Guid id, CancellationToken cancellationToken)
			=> _repo.GetAsync(id, cancellationToken);
	}
}
