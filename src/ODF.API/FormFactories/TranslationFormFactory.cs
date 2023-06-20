using FluentValidation.Results;
using ODF.API.Extensions;
using ODF.API.RequestModels.Forms;
using ODF.API.ResponseModels.Common.Forms;
using ODF.Domain.Constants;

namespace ODF.API.FormFactories
{
	public static class TranslationFormFactory
	{
		public static Form GetChangeTranslationForm(ChangeTranslationForm changeTrans, IEnumerable<ValidationFailure>? errors = null)
		{
			var form = new Form();
			form.AddMember(new("Kód překladu", nameof(ChangeTranslationForm.TranslationCode), FormValueTypes.Text,
				changeTrans.TranslationCode, false, errors?.GetErrorMessage(nameof(ChangeTranslationForm.TranslationCode))));

			form.AddMember(new("Překlad", nameof(ChangeTranslationForm.Text), FormValueTypes.Text,
				changeTrans.Text, true, errors?.GetErrorMessage(nameof(ChangeTranslationForm.Text))));

			form.AddMember(new("Kód země", nameof(ChangeTranslationForm.CountryCode), FormValueTypes.Text,
				changeTrans.CountryCode, false, errors?.GetErrorMessage(nameof(ChangeTranslationForm.CountryCode))));

			return form;
		}
	}
}
