using System.Collections.Generic;
using MediatR;
using ODF.AppLayer.Dtos;

namespace ODF.AppLayer.CQRS.Article.Queries
{
	public class GetArticlesQuery : IRequest<IEnumerable<ArticleDto>>
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
