﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Nest;
using ODF.AppLayer.Repos;
using ODF.Domain.Entities;

namespace ODF.Data.Elastic.Repos.Translations
{
	internal class TranslationRepo : ITranslationRepo
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

			return (await _elasticClient.IndexAsync<Translation>(translation, i => i, cancellationToken)).IsValid;
		}

		public async Task<IEnumerable<Translation>> GetPagedAsync(int size, int offset, int languageId, CancellationToken cancellationToken)
			=> (await _elasticClient.SearchAsync<Translation>(s => s
							.Query(q => q
								.Bool(bq => bq
									.Filter(
										fq => fq.Terms(t => t.Field(f => f.LanguageId).Terms(languageId))
									)
								)
							)
							.Size(size)
							.From(offset * size), cancellationToken)).Documents;

		public async Task<string> GetTranslationAsync(string translationIdentifier, int languageId, CancellationToken cancellationToken)
		{
			var trans = await GetAsync(translationIdentifier, languageId, cancellationToken);

			return trans?.Text ?? string.Empty;
		}

		public async Task<long> GetTranslationsCountAsync(int languageId, CancellationToken cancellationToken)
			=> (await _elasticClient.CountAsync<Translation>(c => c
							.Query(q => q
								.Bool(bq => bq
									.Filter(
										fq => fq.Terms(t => t.Field(f => f.LanguageId).Terms(languageId))
									)
								)
							)
			)).Count;

		public async Task<IEnumerable<Translation>> GetAllAsync(int languageId, CancellationToken cancellationToken)
			=> (await _elasticClient.SearchAsync<Translation>(s => s
							.Query(q => q
								.Bool(bq => bq
									.Filter(
										fq => fq.Terms(t => t.Field(f => f.LanguageId).Terms(languageId))
									)
								)
							)
							.Size(10000)
							, cancellationToken)).Documents;

		public async Task<bool> UpdateOrInsertTransaltionAsync(string translationIdentifier, string text, int languageId, CancellationToken cancellationToken)
		{
			var translation = await GetAsync(translationIdentifier, languageId, cancellationToken);

			if (translation is not null)
			{
				return (await _elasticClient.UpdateByQueryAsync<Translation>(s => s
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
						.Source("ctx._source.text = params.Text;")
						.Params(p => p.Add("Text", text)))
						.Refresh(true)
					)).IsValid;
			}

			return await AddTranslationAsync(translationIdentifier, text, languageId, cancellationToken);
		}

		private async Task<Translation> GetAsync(string translationIdentifier, int languageId, CancellationToken cancellationToken)
		{
			var trans = await _elasticClient.SearchAsync<Translation>(s => s
							.Query(q => q
								.QueryString(qs => qs.Fields(f => f.Field(fi => fi.TranslationCode)).Query(translationIdentifier)))
							.Size(5), cancellationToken);

			return trans.Documents.FirstOrDefault(t => t.LanguageId == languageId);
		}

		public async Task<bool> DeleteTranslationAsync(string translationIdentifier, CancellationToken cancellationToken)
			=> (await _elasticClient.DeleteByQueryAsync<Translation>(q => q
					.Query(rq => rq
						.Match(m => m
						.Field(f => f.TranslationCode)
						.Query(translationIdentifier))
					), cancellationToken
				)).IsValid;
	}
}
