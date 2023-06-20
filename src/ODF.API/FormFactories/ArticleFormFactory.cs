using FluentValidation.Results;
using ODF.API.RequestModels.Forms;
using ODF.API.ResponseModels.Common.Forms;

namespace ODF.API.FormFactories
{
	public static class ArticleFormFactory
	{
		public static Form GetAddArticleForm(AddArticleRequestForm requestform, IEnumerable<ValidationFailure>? errors = null)
		{
			var form = new Form();
			form.AddMember(new("Nadpis", nameof(AddArticleRequestForm.Title), "text", requestform.Title, true,
				errors?.FirstOrDefault(p => p.PropertyName == nameof(AddArticleRequestForm.Title))?.ErrorMessage));

			form.AddMember(new("Kód nadpisu", nameof(AddArticleRequestForm.TitleTranslationCode), "text", requestform.TitleTranslationCode, true,
				errors?.FirstOrDefault(p => p.PropertyName == nameof(AddArticleRequestForm.TitleTranslationCode))?.ErrorMessage));

			form.AddMember(new("Text", nameof(AddArticleRequestForm.Text), "text", requestform.Text, true,
				errors?.FirstOrDefault(p => p.PropertyName == nameof(AddArticleRequestForm.Text))?.ErrorMessage));

			form.AddMember(new("Kód textu", nameof(AddArticleRequestForm.TextTranslationCode), "text", requestform.TextTranslationCode, true,
				errors?.FirstOrDefault(p => p.PropertyName == nameof(AddArticleRequestForm.TextTranslationCode))?.ErrorMessage));

			form.AddMember(new("Stránka", nameof(AddArticleRequestForm.PageId), "number", requestform.PageId, false,
				errors?.FirstOrDefault(p => p.PropertyName == nameof(AddArticleRequestForm.PageId))?.ErrorMessage));

			form.AddMember(new("Obrázek", nameof(AddArticleRequestForm.ImageUrl), "url", requestform.ImageUrl, true,
				 errors?.FirstOrDefault(p => p.PropertyName == nameof(AddArticleRequestForm.ImageUrl))?.ErrorMessage));

			return form;
		}
	}
}
