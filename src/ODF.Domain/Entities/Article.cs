﻿namespace ODF.Domain.Entities
{
	public class Article
	{
		public int Id { get; set; }

		public string TitleTranslationCode { get; set; } = string.Empty;

		public string TextTranslationCode { get; set; } = string.Empty;

		public int PageId { get; set; }

		public string ImageUri { get; set; } = string.Empty;
	}
}
