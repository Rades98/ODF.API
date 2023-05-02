using MediatR;
using ODF.AppLayer.Dtos;

namespace ODF.AppLayer.CQRS.Article.Queries
{
	public class GetArticleQuery : IRequest<ArticleDto>
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
