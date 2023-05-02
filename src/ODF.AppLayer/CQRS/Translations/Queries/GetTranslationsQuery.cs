using System.Collections.Generic;
using MediatR;
using ODF.AppLayer.Dtos;

namespace ODF.AppLayer.CQRS.Translations.Queries
{
	public class GetTranslationsQuery : IRequest<TranslationsDto>
	{
		public GetTranslationsQuery(string countryCode, int size, int offset)
		{
			CountryCode = countryCode;
			Size = size;
			Offset = offset;
		}

		public string CountryCode { get; }

		public int Size { get; }

		public int Offset { get; }
	}
}
