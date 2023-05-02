﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Nest;
using ODF.Data.Contracts.Entities;
using ODF.Data.Contracts.Interfaces;

namespace ODF.Data.Elastic
{
	public class TranslationRepo : ITranslationRepo
	{
		private readonly IElasticClient _elasticClient;

		public TranslationRepo(IElasticClient elasticClient)
		{
			_elasticClient = elasticClient ?? throw new ArgumentNullException(nameof(elasticClient));
		}

		public async Task<bool> AddTranslationAsync(string translationIdentifier, string text, int languageId, CancellationToken cancellationToken)
		{
			var translation = new Translation()
			{
				LanguageId = languageId,
				TranslationCode = translationIdentifier,
				Text = text,
			};

			return (await _elasticClient.IndexAsync(translation, i => i.Index("translations"), cancellationToken)).IsValid;
		}

		public async Task<IEnumerable<Translation>> GetPagedAsync(int size, int offset, int languageId, CancellationToken cancellationToken)
			=> (await _elasticClient.SearchAsync<Translation>(s => s
							.Index("translations")
							.Query(q => q
								.Bool(bq => bq
									.Filter(
										fq => fq.Terms(t => t.Field(f => f.LanguageId).Terms(languageId))
									)
								)
							)
							.Size(size)
							.From(offset), cancellationToken)).Documents;

		public async Task<string> GetTranslationAsync(string translationIdentifier, int languageId, CancellationToken cancellationToken)
		{
			var translation = await GetAsync(translationIdentifier, languageId, cancellationToken);

			if(translation is null)
			{
				return string.Empty;
			}

			return translation.Text;
		}

		public async Task<string> GetTranslationOrDefaultTextAsync(string translationIdentifier, string defaultTranslation, int languageId, CancellationToken cancellationToken)
		{
			var translation = await GetAsync(translationIdentifier, languageId, cancellationToken);

			if (translation is null)
			{
				if (await AddTranslationAsync(translationIdentifier, defaultTranslation, languageId, cancellationToken))
				{
					return defaultTranslation;
				}
			}

			return translation.Text;
		}

		public async Task<long> GetTranslationsCountAsync(int languageId, CancellationToken cancellationToken)
			=> (await _elasticClient.CountAsync<Translation>(c => c
							.Index("translations")
							.Query(q => q
								.Bool(bq => bq
									.Filter(
										fq => fq.Terms(t => t.Field(f => f.LanguageId).Terms(languageId))
									)
								)
							)
			)).Count;

		public async Task<bool> UpdateOrInsertTransaltionAsync(string translationIdentifier, string text, int languageId, CancellationToken cancellationToken)
		{
			var translation = await GetAsync(translationIdentifier, languageId, cancellationToken);

			if(translation is not null)
			{
				var scriptParams = new Dictionary<string, object> { { "Text", text } };

				return (await _elasticClient.UpdateByQueryAsync<Translation>(s => s
				.Index("translations")
				.Query(q => q
					.Bool(bq => bq
						.Filter(
							fq => fq.Terms(t => t.Field(f => f.TranslationCode).Terms(translationIdentifier)),
							fq => fq.Terms(t => t.Field(f => f.LanguageId).Terms(languageId))
						)
					)
				)
				.Size(1)
				.Script(s => s
					.Source("ctx._source.propertyName = params.paramName;")
					.Params(scriptParams))
				)).IsValid;
			}

			return await AddTranslationAsync(translationIdentifier, text, languageId, cancellationToken);
		}

		private async Task<Translation>GetAsync(string translationIdentifier, int languageId, CancellationToken cancellationToken)
			=> (await _elasticClient.SearchAsync<Translation>(s => s
							.Index("translations")
							.Query(q => q
								.Bool(bq => bq
									.Filter(
										fq => fq.Terms(t => t.Field(f => f.TranslationCode).Terms(translationIdentifier)),
										fq => fq.Terms(t => t.Field(f => f.LanguageId).Terms(languageId))
									)
								)
							)
							.Size(1), cancellationToken)).Documents.FirstOrDefault();
	}
}
