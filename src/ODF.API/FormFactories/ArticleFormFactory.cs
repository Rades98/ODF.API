using FluentValidation.Results;
using ODF.API.Extensions;
using ODF.API.RequestModels.Forms;
using ODF.API.ResponseModels.Common.Forms;
using ODF.Domain.Constants;

namespace ODF.API.FormFactories
{
	public static class ArticleFormFactory
	{
		public static Form GetAddArticleForm(AddArticleRequestForm requestform, IEnumerable<ValidationFailure>? errors = null)
		{
			var form = new Form();
			form.AddMember(new("Nadpis", nameof(AddArticleRequestForm.Title), FormValueTypes.Text, requestform.Title, true,
				errors?.GetErrorMessage(nameof(AddArticleRequestForm.Title))));

			form.AddMember(new("Kód nadpisu", nameof(AddArticleRequestForm.TitleTranslationCode), FormValueTypes.Text, requestform.TitleTranslationCode, true,
				errors?.GetErrorMessage(nameof(AddArticleRequestForm.TitleTranslationCode))));

			form.AddMember(new("Text", nameof(AddArticleRequestForm.Text), FormValueTypes.Text, requestform.Text, true,
				errors?.GetErrorMessage(nameof(AddArticleRequestForm.Text))));

			form.AddMember(new("Kód textu", nameof(AddArticleRequestForm.TextTranslationCode), FormValueTypes.Text, requestform.TextTranslationCode, true,
				errors?.GetErrorMessage(nameof(AddArticleRequestForm.TextTranslationCode))));

			form.AddMember(new("Stránka", nameof(AddArticleRequestForm.PageId), FormValueTypes.Number, requestform.PageId, false,
				errors?.GetErrorMessage(nameof(AddArticleRequestForm.PageId))));

			form.AddMember(new("Obrázek", nameof(AddArticleRequestForm.ImageUrl), FormValueTypes.Url, requestform.ImageUrl, true,
				 errors?.GetErrorMessage(nameof(AddArticleRequestForm.ImageUrl))));

			return form;
		}
	}
}
