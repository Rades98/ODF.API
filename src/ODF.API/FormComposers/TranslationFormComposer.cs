using FluentValidation.Results;
using ODF.API.Extensions;
using ODF.API.RequestModels.Forms;
using ODF.API.ResponseModels.Common.Forms;
using ODF.AppLayer.CQRS.Interfaces.Translations;
using ODF.Domain.Constants;

namespace ODF.API.FormComposers
{
	public static class TranslationFormComposer
	{
		public static Form GetChangeTranslationForm(UpdateTranslationForm changeTrans, IEnumerable<ValidationFailure>? errors = null)
		{
			var form = new Form();
			form.AddMember(new("Kód překladu", nameof(IUpdateTranslation.TranslationCode), FormValueTypes.Text, changeTrans.TranslationCode,
				false, errors?.GetErrorMessage(nameof(IUpdateTranslation.TranslationCode))));

			form.AddMember(new("Překlad", nameof(IUpdateTranslation.Text), FormValueTypes.Text, changeTrans.Text,
				true, errors?.GetErrorMessage(nameof(IUpdateTranslation.Text))));

			form.AddMember(new("Kód země", nameof(IUpdateTranslation.CountryCode), FormValueTypes.Text, changeTrans.CountryCode,
				false, errors?.GetErrorMessage(nameof(IUpdateTranslation.CountryCode))));

			return form;
		}
	}
}
