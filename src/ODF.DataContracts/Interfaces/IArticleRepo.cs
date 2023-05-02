using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ODF.Data.Contracts.Entities;

namespace ODF.Data.Contracts.Interfaces
{
	public interface IArticleRepo
	{
		Task<Article?> GetArticleAsync(int id, CancellationToken cancellationToken);

		Task<IEnumerable<Article>> GetArticlesPaginatedAsync(int pageId, int size, int offset, CancellationToken cancellationToken);

		Task<bool> AddArticleAsync(string titleTranslationCode, string textTransaltionCode, int pageId, string imageUrl, CancellationToken cancellationToken);
	}
}
