using System.Collections.Generic;
using ODF.AppLayer.Dtos;
using ODF.AppLayer.Mediator;

namespace ODF.AppLayer.CQRS.Article.Queries
{
	public sealed class GetArticlesQuery : IQuery<IEnumerable<ArticleDto>>
	{
		public GetArticlesQuery(int offset, int size, int pageId, string countryCode)
		{
			Offset = offset;
			Size = size;
			PageId = pageId;
			CountryCode = countryCode;
		}

		public int Offset { get; }

		public int Size { get; }

		public int PageId { get; }

		public string CountryCode { get; }
	}
}
