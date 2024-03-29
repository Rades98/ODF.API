﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Nest;
using ODF.AppLayer.Repos;
using ODF.Domain.Entities;

namespace ODF.Data.Elastic.Repos.Articles
{
	internal class ArticleRepo : IArticleRepo
	{
		private readonly IElasticClient _elasticClient;

		public ArticleRepo(IElasticClient elasticClient)
		{
			_elasticClient = elasticClient ?? throw new ArgumentNullException(nameof(elasticClient));
		}

		public async Task<bool> AddArticleAsync(string titleTranslationCode, string textTranslationCode, int pageId, Uri imageUrl, CancellationToken cancellationToken)
		{
			var last = await GetLast(cancellationToken);
			int id = last is not null ? last.Id + 1 : 0;

			var article = new Article()
			{
				Id = id,
				PageId = pageId,
				TextTranslationCode = textTranslationCode,
				TitleTranslationCode = titleTranslationCode,
				ImageUri = imageUrl.ToString() ?? "https://placehold.co/600x400",
			};

			var result = await _elasticClient.IndexAsync<Article>(article, i => i, cancellationToken);

			return result.IsValid;
		}

		public async Task<Article> GetArticleAsync(int id, CancellationToken cancellationToken)
			=> (await _elasticClient.SearchAsync<Article>(s => s
							.Query(fq => fq.Terms(t => t.Field(f => f.Id).Terms(id)))
							.Size(1), cancellationToken)).Documents.FirstOrDefault();

		public async Task<IEnumerable<Article>> GetArticlesPaginatedAsync(int pageId, int size, int offset, CancellationToken cancellationToken)
			=> (await _elasticClient.SearchAsync<Article>(s => s
							.Query(fq => fq.Terms(t => t.Field(f => f.PageId).Terms(pageId)))
							.Sort(s => s.Descending(f => f.Id))
							.From(offset)
							.Size(size), cancellationToken))
							.Documents;

		private async Task<Article> GetLast(CancellationToken cancellationToken)
			=> (await _elasticClient.SearchAsync<Article>(s => s
					.Sort(s => s.Descending(f => f.Id))
					.Size(1)
					, cancellationToken)
				).Documents.FirstOrDefault();
	}
}
