using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using ODF.AppLayer.Repos;
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

		public Task<bool> AddArticleAsync(string titleTranslationCode, string textTransaltionCode, int pageId, Uri imageUrl, CancellationToken cancellationToken)
			=> _repo.AddArticleAsync(titleTranslationCode, textTransaltionCode, pageId, imageUrl, cancellationToken);

		public Task<Article> GetArticleAsync(int id, CancellationToken cancellationToken)
			=> _repo.GetArticleAsync(id, cancellationToken);

		public async Task<IEnumerable<Article>> GetArticlesPaginatedAsync(int pageId, int size, int offset, CancellationToken cancellationToken)
		{
			string cacheKey = $"{nameof(Article)}s_{pageId}_paged{size}_{offset}";

			object cachedResponse = _cache.Get(cacheKey);

			if (cachedResponse != null)
			{
				var res = JsonConvert.DeserializeObject<IEnumerable<Article>>(Encoding.Default.GetString((byte[])cachedResponse));
				if (res is not null)
				{
					return res;
				}
			}

			var response = await _repo.GetArticlesPaginatedAsync(pageId, size, offset, cancellationToken);

			_cache.Set(cacheKey, Encoding.Default.GetBytes(JsonConvert.SerializeObject(response)), new MemoryCacheEntryOptions()
			{
				AbsoluteExpiration = DateTime.Now.AddMinutes(60)
			});

			return response;
		}
	}
}
