using MediatR;

namespace ODF.AppLayer.CQRS.Translations.Queries
{
	public class GetTranslationQuery : IRequest<string>
	{
		public GetTranslationQuery(string defaultText, string translationIdentifier, string countryCode)
		{
			DefaultText = defaultText;
			TranslationIdentifier = translationIdentifier;
			CountryCode = countryCode;
		}

		public string DefaultText { get; }

		public string TranslationIdentifier { get; }

		public string CountryCode { get; }
	}
}
