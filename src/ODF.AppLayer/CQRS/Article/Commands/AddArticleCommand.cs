using System;
using ODF.AppLayer.CQRS.Interfaces.Article;
using ODF.AppLayer.Dtos.Validation;
using ODF.AppLayer.Mediator;

namespace ODF.AppLayer.CQRS.Article.Commands
{
	public sealed class AddArticleCommand : ICommand<ValidationDto>, IAddArticle
	{
		public AddArticleCommand(string titleTransaltionCode, string title, string textTranslationCode, string text, int pageId, string countryCode, Uri imageUri = null)
		{
			TitleTranslationCode = titleTransaltionCode;
			Title = title;
			TextTranslationCode = textTranslationCode;
			Text = text;
			PageId = pageId;
			CountryCode = countryCode;
			ImageUri = imageUri;
		}

		public string TitleTranslationCode { get; }

		public string Title { get; }

		public string TextTranslationCode { get; }

		public string Text { get; }

		public int PageId { get; }

		public string CountryCode { get; }

		public Uri ImageUri { get; } = null;
	}
}
