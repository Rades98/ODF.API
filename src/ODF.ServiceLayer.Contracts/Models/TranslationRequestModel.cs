
namespace ODF.ServiceLayer.Contracts.Models

{
    public class TranslationRequestModel 
	{
		public TranslationRequestModel(string defaultText, string translationIdentifier, string countryCode)
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
