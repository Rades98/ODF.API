using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Nest;
using ODF.Data.Contracts.Entities;
using ODF.Data.Contracts.Interfaces;

namespace ODF.Data.Elastic.Repos.Articles
{
	internal class ArticleRepo : IArticleRepo
	{
		private readonly IElasticClient _elasticClient;

		public ArticleRepo(IElasticClient elasticClient)
		{
			_elasticClient = elasticClient ?? throw new ArgumentNullException(nameof(elasticClient));
		}

		public async Task<bool> AddArticleAsync(string titleTranslationCode, string textTranslationCode, int pageId, string imageUrl, CancellationToken cancellationToken)
		{
			var last = await GetLast(cancellationToken);
			int id = 0;
			if (last is not null)
			{
				id = last.Id + 1;
			}

			var article = new Article()
			{
				Id = id,
				PageId = pageId,
				TextTranslationCode = textTranslationCode,
				TitleTranslationCode = titleTranslationCode,
				ImageUri = imageUrl,
			};

			return (await _elasticClient.IndexAsync(article, i => i, cancellationToken)).IsValid;
		}

		public async Task<Article?> GetArticleAsync(int id, CancellationToken cancellationToken)
			=> (await _elasticClient.SearchAsync<Article>(s => s
							.Query(q => q
								.Bool(bq => bq
									.Filter(
										fq => fq.Terms(t => t.Field(f => f.Id).Terms(id))
									)
								)
							)
							.Size(1), cancellationToken)).Documents.FirstOrDefault();

		public async Task<IEnumerable<Article>> GetArticlesPaginatedAsync(int pageId, int size, int offset, CancellationToken cancellationToken)
		{
			var res = (await _elasticClient.SearchAsync<Article>(s => s
							.Query(q => q
								.Bool(bq => bq
									.Filter(
										fq => fq.Terms(t => t.Field(f => f.PageId).Terms(pageId))
									)
								)
							)
							.Sort(s => s.Descending(f => f.Id))
							.From(offset)
							.Size(size), cancellationToken))
							.Documents;

			return res;
		}

		private async Task<Article> GetLast(CancellationToken cancellationToken)
			=> (await _elasticClient.SearchAsync<Article>(s => s
					.Sort(s => s.Descending(f => f.Id))
					.Size(1)
					, cancellationToken)
				).Documents.FirstOrDefault();
	}
}
