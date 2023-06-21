using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ODF.Domain.Entities;

namespace ODF.AppLayer.Repos
{
	public interface IArticleRepo
	{
		Task<Article> GetArticleAsync(int id, CancellationToken cancellationToken);

		Task<IEnumerable<Article>> GetArticlesPaginatedAsync(int pageId, int size, int offset, CancellationToken cancellationToken);

		Task<bool> AddArticleAsync(string titleTranslationCode, string textTranslationCode, int pageId, Uri imageUrl, CancellationToken cancellationToken);
	}
}
