using MediatR;

namespace ODF.AppLayer.CQRS.Translations.Commands
{
	public class ModifyTransaltionCommand : IRequest<bool>
	{
		public ModifyTransaltionCommand(string countryCode, string translationCode, string text)
		{
			CountryCode = countryCode;
			TranslationCode = translationCode;
			Text = text;
		}

		public string CountryCode { get; }

		public string TranslationCode { get;} 

		public string Text { get; }
	}
}
