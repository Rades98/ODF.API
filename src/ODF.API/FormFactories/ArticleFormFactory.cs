using ODF.API.ResponseModels.Common.Forms;

namespace ODF.API.FormFactories
{
	public static class ArticleFormFactory
	{
		public static Form GetAddArticleForm(string title, string titleTranslationCode, string text, string textTranslationCode, int pageId, Uri? imageUrl = null)
		{
			var form = new Form();
			form.AddMember(new("Nadpis", nameof(title), "text", title, true));
			form.AddMember(new("Kód nadpisu", nameof(titleTranslationCode), "text", titleTranslationCode, true));

			form.AddMember(new("Text", nameof(text), "text", text, true));
			form.AddMember(new("Kód textu", nameof(textTranslationCode), "text", textTranslationCode, true));

			form.AddMember(new("Stránka", nameof(pageId), "number", pageId));

			form.AddMember(new("Obrázek", nameof(imageUrl), "url", imageUrl ?? new("https://placehold.co/600x400"), true));

			return form;
		}
	}
}
