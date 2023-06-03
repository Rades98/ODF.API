using System;
using MediatR;

namespace ODF.AppLayer.CQRS.Article.Commands
{
	public class AddArticleCommand : IRequest<bool>
	{
		public AddArticleCommand(string titleTransaltionCode, string title, string textTranslationCode, string text, int pageId, string countryCode, Uri imageUri = null)
		{
			TitleTransaltionCode = titleTransaltionCode;
			Title = title;
			TextTransaltionCode = textTranslationCode;
			Text = text;
			PageId = pageId;
			CountryCode = countryCode;
			ImageUri = imageUri;
		}

		public string TitleTransaltionCode { get; }

		public string Title { get; }

		public string TextTransaltionCode { get; }

		public string Text { get; }

		public int PageId { get; }

		public string CountryCode { get; }

		public Uri ImageUri { get; } = null;
	}
}
