using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ODF.AppLayer.Dtos;
using ODF.AppLayer.Exceptions;
using ODF.AppLayer.Mapping;
using ODF.AppLayer.Repos;
using ODF.AppLayer.Services.Interfaces;
using ODF.Domain;

namespace ODF.AppLayer.Services
{
	internal class TranslationsProvider : ITranslationsProvider
	{
		private readonly ITranslationRepo _repo;

		public TranslationsProvider(ITranslationRepo repo)
		{
			_repo = repo ?? throw new ArgumentNullException(nameof(repo));
		}

		public async Task<IReadOnlyList<TranslationDto>> GetTranslationsAsync(string countryCode, CancellationToken cancellationToken)
		{
			if (Languages.TryParse(countryCode, out var lang))
			{
				var trans = await _repo.GetAllAsync(lang.Id, cancellationToken);

				return trans.Select(t => t.ToDto()).ToList();
			}

			throw new UnsupportedLanguageException($"No language provided for country code: {countryCode}");
		}
	}
}
