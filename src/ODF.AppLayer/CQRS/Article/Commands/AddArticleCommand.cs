using System;
using ODF.AppLayer.CQRS.Interfaces.Article;
using ODF.AppLayer.Dtos.Validation;
using ODF.AppLayer.Mediator;

namespace ODF.AppLayer.CQRS.Article.Commands
{
	public sealed class AddArticleCommand : ICommand<ValidationDto>, IAddArticle
	{
		public AddArticleCommand(IAddArticle input, string countryCode)
		{
			TitleTranslationCode = input.TitleTranslationCode;
			Title = input.Title;
			TextTranslationCode = input.TextTranslationCode;
			Text = input.Text;
			PageId = input.PageId;
			CountryCode = countryCode;
			ImageUri = input.ImageUri;
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
