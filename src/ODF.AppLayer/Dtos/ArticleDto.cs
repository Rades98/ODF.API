using System;

namespace ODF.AppLayer.Dtos
{
	public class ArticleDto
	{
		public int Id { get; set; }

		public string Title { get; set; } = string.Empty;

		public string Text { get; set; } = string.Empty;

		public string ImageUri { get; set; }

	}
}
