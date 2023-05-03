using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Nest;
using Newtonsoft.Json;
using ODF.Data.Contracts.Entities;
using ODF.Data.Contracts.Interfaces;

namespace ODF.Data.Elastic.Repos.Translations
{
	internal class TranslationRepoCache : ITranslationRepo
	{
		private readonly IMemoryCache _cache;
		private readonly ITranslationRepo _repo;

		public TranslationRepoCache(IMemoryCache cache, ITranslationRepo repo)
		{
			_cache = cache ?? throw new ArgumentNullException(nameof(cache));
			_repo = repo ?? throw new ArgumentNullException(nameof(repo));
		}

		public Task<bool> AddTranslationAsync(string translationIdentifier, string text, int languageId, CancellationToken cancellationToken)
			=> _repo.AddTranslationAsync(translationIdentifier, text, languageId, cancellationToken);

		public Task<bool> UpdateOrInsertTransaltionAsync(string translationIdentifier, string text, int languageId, CancellationToken cancellationToken)
			=> _repo.UpdateOrInsertTransaltionAsync(translationIdentifier, text, languageId, cancellationToken);

		public async Task<IEnumerable<Translation>> GetPagedAsync(int size, int offset, int languageId, CancellationToken cancellationToken)
		{
			var cacheKey = $"{nameof(Translation)}s_paged{size}_{offset}_lan{languageId}";

			var cachedResponse = _cache.Get(cacheKey);

			if (cachedResponse != null)
			{
				var res = JsonConvert.DeserializeObject<IEnumerable<Translation>>(Encoding.Default.GetString((byte[])cachedResponse));
				if (res is not null)
				{
					return res;
				}
			}

			var response = await _repo.GetPagedAsync(size, offset, languageId, cancellationToken);

			_cache.Set(cacheKey, Encoding.Default.GetBytes(JsonConvert.SerializeObject(response)), new MemoryCacheEntryOptions()
			{
				AbsoluteExpiration = DateTime.Now.AddMinutes(60)
			});

			return response;
		}

		public async Task<string> GetTranslationAsync(string translationIdentifier, int languageId, CancellationToken cancellationToken)
		{
			var cacheKey = $"{nameof(Translation)}_{translationIdentifier}_{languageId}";

			var cachedResponse = _cache.Get(cacheKey);

			if (cachedResponse != null)
			{
				var res = JsonConvert.DeserializeObject<string>(Encoding.Default.GetString((byte[])cachedResponse));
				if (res is not null)
				{
					return res;
				}
			}

			var response = await _repo.GetTranslationAsync(translationIdentifier, languageId, cancellationToken);

			_cache.Set(cacheKey, Encoding.Default.GetBytes(JsonConvert.SerializeObject(response)), new MemoryCacheEntryOptions()
			{
				AbsoluteExpiration = DateTime.Now.AddMinutes(60)
			});

			return response;
		}

		public async Task<string> GetTranslationOrDefaultTextAsync(string translationIdentifier, string defaultTranslation, int languageId, CancellationToken cancellationToken)
		{
			var cacheKey = $"{nameof(Translation)}_{translationIdentifier}_{languageId}_{defaultTranslation}";

			var cachedResponse = _cache.Get(cacheKey);

			if (cachedResponse != null)
			{
				var res = JsonConvert.DeserializeObject<string>(Encoding.Default.GetString((byte[])cachedResponse));
				if (res is not null)
				{
					return res;
				}
			}

			var response = await _repo.GetTranslationOrDefaultTextAsync(translationIdentifier, defaultTranslation, languageId, cancellationToken);

			_cache.Set(cacheKey, Encoding.Default.GetBytes(JsonConvert.SerializeObject(response)), new MemoryCacheEntryOptions()
			{
				AbsoluteExpiration = DateTime.Now.AddMinutes(60)
			});

			return response;
		}

		public async Task<long> GetTranslationsCountAsync(int languageId, CancellationToken cancellationToken)
		{
			var cacheKey = $"{nameof(Translation)}_count_{languageId}";

			var cachedResponse = _cache.Get(cacheKey);

			if (cachedResponse != null)
			{
				var res = JsonConvert.DeserializeObject<long>(Encoding.Default.GetString((byte[])cachedResponse));
				return res;
			}

			var response = await _repo.GetTranslationsCountAsync(languageId, cancellationToken);

			_cache.Set(cacheKey, Encoding.Default.GetBytes(JsonConvert.SerializeObject(response)), new MemoryCacheEntryOptions()
			{
				AbsoluteExpiration = DateTime.Now.AddMinutes(60)
			});

			return response;
		}
	}
}
