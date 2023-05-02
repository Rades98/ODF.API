using ODF.API.ResponseModels.Common.Forms;

namespace ODF.API.FormFactories
{
	public static class ArticleFormFactory
	{
		public static Form GetAddArticleForm(string title, string titleTranslationCode, string text, string textTranslationCode, int pageId, string countryCode, string imageUrl = "https://placehold.co/600x400")
		{
			var form = new Form();
			form.AddMember(new("Nadpis", nameof(title), title.GetType().Name, title, true));
			form.AddMember(new("Kód nadpisu", nameof(titleTranslationCode), titleTranslationCode.GetType().Name, titleTranslationCode, true));

			form.AddMember(new("Text", nameof(text), text.GetType().Name, text, true));
			form.AddMember(new("Kód textu", nameof(textTranslationCode), textTranslationCode.GetType().Name, textTranslationCode, true));

			form.AddMember(new("Stránka", nameof(pageId), pageId.GetType().Name, pageId));
			form.AddMember(new("Země", nameof(countryCode), countryCode.GetType().Name, countryCode));
			
			form.AddMember(new("Obrázek", nameof(imageUrl), imageUrl.GetType().Name, imageUrl, true));

			return form;
		}
	}
}
