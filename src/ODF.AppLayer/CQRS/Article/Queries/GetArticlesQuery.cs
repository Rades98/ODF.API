using System.Collections.Generic;
using ODF.AppLayer.Dtos;
using ODF.AppLayer.Mediator;

namespace ODF.AppLayer.CQRS.Article.Queries
{
	public sealed record GetArticlesQuery(int Offset, int Size, int PageId, string CountryCode) : IQuery<IEnumerable<ArticleDto>>;
}
