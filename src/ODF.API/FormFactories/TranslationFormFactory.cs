using ODF.API.ResponseModels.Common.Forms;

namespace ODF.API.FormFactories
{
	public static class TranslationFormFactory
	{
		public static Form GetChangeTranslationForm(string translationCode, string text, string countryCode)
		{
			var form = new Form();
			form.AddMember(new("Kód překladu", nameof(translationCode), "text", translationCode));
			form.AddMember(new("Překlad", nameof(text), "text", text, true));
			form.AddMember(new("Kód země", nameof(countryCode), "text", countryCode));

			return form;
		}
	}
}
