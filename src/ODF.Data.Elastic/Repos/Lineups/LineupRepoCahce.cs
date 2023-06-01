﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Nest;
using Newtonsoft.Json;
using ODF.Data.Contracts.Entities;
using ODF.Data.Contracts.Interfaces;

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
			var cacheKey = $"{nameof(LineupItem)}s";

			var cachedResponse = _cache.Get(cacheKey);

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
	}
}