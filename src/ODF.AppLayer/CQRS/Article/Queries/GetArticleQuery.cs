using ODF.AppLayer.Dtos;
using ODF.AppLayer.Mediator;

namespace ODF.AppLayer.CQRS.Article.Queries
{
	public class GetArticleQuery : IQuery<ArticleDto>
	{
		public GetArticleQuery(int id, string countryCode)
		{
			Id = id;
			CountryCode = countryCode;
		}

		public int Id { get; }

		public string CountryCode { get; }
	}
}
