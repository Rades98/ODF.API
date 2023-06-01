using System;

namespace ODF.Data.Contracts.Entities
{
	public class Article
	{
		public int Id { get; set; }

		public string TitleTranslationCode { get; set; } = string.Empty;

		public string TextTranslationCode { get; set; } = string.Empty;

		public int PageId { get; set; }

		public Uri? ImageUri { get; set; }
	}
}
