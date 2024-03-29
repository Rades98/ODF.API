﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using ODF.AppLayer.Repos;
using ODF.Data.Elastic.Settings;
using ODF.Domain.Entities;

namespace ODF.Data.Elastic.Repos.Articles
{
	internal class ArticleRepoCache : IArticleRepo
	{
		private readonly IMemoryCache _cache;
		private readonly IArticleRepo _repo;

		public ArticleRepoCache(IMemoryCache cache, IArticleRepo repo)
		{
			_cache = cache ?? throw new ArgumentNullException(nameof(cache));
			_repo = repo ?? throw new ArgumentNullException(nameof(repo));
		}

		public Task<bool> AddArticleAsync(string titleTranslationCode, string textTranslationCode, int pageId, Uri imageUrl, CancellationToken cancellationToken)
			=> _repo.AddArticleAsync(titleTranslationCode, textTranslationCode, pageId, imageUrl, cancellationToken);

		public async Task<Article> GetArticleAsync(int id, CancellationToken cancellationToken)
		{
			string cacheKey = $"{nameof(Article)}_{id}";

			object cachedResponse = _cache.Get(cacheKey);

			if (cachedResponse != null)
			{
				var res = JsonConvert.DeserializeObject<Article>(Encoding.Default.GetString((byte[])cachedResponse));
				if (res is not null)
				{
					return res;
				}
			}

			var response = await _repo.GetArticleAsync(id, cancellationToken);

			_cache.Set(cacheKey, Encoding.Default.GetBytes(JsonConvert.SerializeObject(response)), CacheSetting.MemoryCacheEntryOptions);

			return response;
		}

		public Task<IEnumerable<Article>> GetArticlesPaginatedAsync(int pageId, int size, int offset, CancellationToken cancellationToken)
			=> _repo.GetArticlesPaginatedAsync(pageId, size, offset, cancellationToken);
	}
}
