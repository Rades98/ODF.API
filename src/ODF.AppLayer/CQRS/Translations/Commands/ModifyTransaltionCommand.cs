using ODF.AppLayer.Dtos.Validation;
using ODF.AppLayer.Mediator;

namespace ODF.AppLayer.CQRS.Translations.Commands
{
	public sealed class ModifyTransaltionCommand : ICommand<ValidationDto>
	{
		public ModifyTransaltionCommand(string countryCode, string translationCode, string text)
		{
			CountryCode = countryCode;
			TranslationCode = translationCode;
			Text = text;
		}

		public string CountryCode { get; }

		public string TranslationCode { get; }

		public string Text { get; }
	}
}
