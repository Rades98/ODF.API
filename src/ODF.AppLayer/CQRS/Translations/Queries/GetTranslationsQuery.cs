using ODF.AppLayer.Dtos;
using ODF.AppLayer.Mediator;

namespace ODF.AppLayer.CQRS.Translations.Queries
{
	public class GetTranslationsQuery : IQuery<TranslationsDto>
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
