using ODF.AppLayer.CQRS.Interfaces.Translations;
using ODF.AppLayer.Dtos.Validation;
using ODF.AppLayer.Mediator;

namespace ODF.AppLayer.CQRS.Translations.Commands
{
	public sealed class UpdateTransaltionCommand : ICommand<ValidationDto>, IUpdateTranslation
	{
		public UpdateTransaltionCommand(string countryCode, string translationCode, string text)
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
