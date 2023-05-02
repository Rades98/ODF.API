using ODF.API.ResponseModels.Common.Forms;

namespace ODF.API.FormFactories
{
	public static class TranslationFormFactory
	{
		public static Form GetChangeTranslationForm(string translationCode, string text, string countryCode)
		{
			var form = new Form();
			form.AddMember(new("Kód překladu", nameof(translationCode), typeof(string).Name, translationCode));
			form.AddMember(new("Překlad", nameof(text), typeof(string).Name, text, true));
			form.AddMember(new("Kód země", nameof(countryCode), typeof(string).Name, countryCode));

			return form;
		}
	}
}
