using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using ODF.AppLayer.Repos;
using ODF.Domain.Entities;

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

		public Task<IEnumerable<Translation>> GetPagedAsync(int size, int offset, int languageId, CancellationToken cancellationToken)
			=> _repo.GetPagedAsync(size, offset, languageId, cancellationToken);

		public Task<string> GetTranslationAsync(string translationIdentifier, int languageId, CancellationToken cancellationToken)
			=> _repo.GetTranslationAsync(translationIdentifier, languageId, cancellationToken);

		public Task<long> GetTranslationsCountAsync(int languageId, CancellationToken cancellationToken)
			=> _repo.GetTranslationsCountAsync(languageId, cancellationToken);

		public async Task<IEnumerable<Translation>> GetAllAsync(int languageId, CancellationToken cancellationToken)
		{
			string cacheKey = $"{nameof(Translation)}s_{languageId}";

			object cachedResponse = _cache.Get(cacheKey);

			if (cachedResponse != null)
			{
				var res = JsonConvert.DeserializeObject<IEnumerable<Translation>>(Encoding.Default.GetString((byte[])cachedResponse));
				if (res is not null)
				{
					return res;
				}
			}

			var response = await _repo.GetAllAsync(languageId, cancellationToken).ConfigureAwait(false);

			_cache.Set(cacheKey, Encoding.Default.GetBytes(JsonConvert.SerializeObject(response)), new MemoryCacheEntryOptions()
			{
				AbsoluteExpiration = DateTime.Now.AddMinutes(60)
			});

			return response;
		}

		public Task<bool> DeleteTranslationAsync(string translationIdentifier, CancellationToken cancellationToken)
			=> _repo.DeleteTranslationAsync(translationIdentifier, cancellationToken);
	}
}
