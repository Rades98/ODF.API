using ODF.AppLayer.Dtos;
using ODF.AppLayer.Mediator;

namespace ODF.AppLayer.CQRS.Article.Queries
{
	public sealed record GetArticleQuery(int Id, string CountryCode) : IQuery<ArticleDto>;
}
