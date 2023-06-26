using System;

namespace ODF.AppLayer.CQRS.Interfaces.Article
{
	public interface IAddArticle
	{
		public string TitleTranslationCode { get; }

		public string Title { get; }

		public string TextTranslationCode { get; }

		public string Text { get; }

		public int PageId { get; }

		public Uri ImageUri { get; }
	}
}
