using FluentValidation.Results;
using ODF.API.Extensions;
using ODF.API.RequestModels.Forms;
using ODF.API.ResponseModels.Common.Forms;
using ODF.AppLayer.CQRS.Interfaces.Article;
using ODF.Domain.Constants;

namespace ODF.API.FormComposers
{
	public static class ArticleFormComposer
	{
		public static Form GetAddArticleForm(AddArticleRequestForm requestform, IEnumerable<ValidationFailure>? errors = null)
		{
			var form = new Form();
			form.AddMember(new("Nadpis", nameof(IAddArticle.Title), FormValueTypes.Text, requestform.Title, true,
				errors?.GetErrorMessage(nameof(IAddArticle.Title))));

			form.AddMember(new("Kód nadpisu", nameof(IAddArticle.TitleTranslationCode), FormValueTypes.Text, requestform.TitleTranslationCode, true,
				errors?.GetErrorMessage(nameof(IAddArticle.TitleTranslationCode))));

			form.AddMember(new("Text", nameof(IAddArticle.Text), FormValueTypes.FormatedText, requestform.Text, true,
				errors?.GetErrorMessage(nameof(IAddArticle.Text))));

			form.AddMember(new("Kód textu", nameof(IAddArticle.TextTranslationCode), FormValueTypes.Text, requestform.TextTranslationCode, true,
				errors?.GetErrorMessage(nameof(IAddArticle.TextTranslationCode))));

			form.AddMember(new("Stránka", nameof(IAddArticle.PageId), FormValueTypes.Number, requestform.PageId, false,
				errors?.GetErrorMessage(nameof(IAddArticle.PageId))));

			form.AddMember(new("Obrázek", nameof(IAddArticle.ImageUri), FormValueTypes.Url, requestform.ImageUri, true,
				 errors?.GetErrorMessage(nameof(IAddArticle.ImageUri))));

			return form;
		}
	}
}
