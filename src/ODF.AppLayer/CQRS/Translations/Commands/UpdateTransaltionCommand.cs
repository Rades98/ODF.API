using ODF.AppLayer.CQRS.Interfaces.Translations;
using ODF.AppLayer.Dtos.Validation;
using ODF.AppLayer.Mediator;

namespace ODF.AppLayer.CQRS.Translations.Commands
{
	public sealed class UpdateTransaltionCommand : ICommand<ValidationDto>, IUpdateTranslation
	{
		public UpdateTransaltionCommand(IUpdateTranslation input)
		{
			CountryCode = input.CountryCode;
			TranslationCode = input.TranslationCode;
			Text = input.Text;
		}

		public string CountryCode { get; }

		public string TranslationCode { get; }

		public string Text { get; }
	}
}
