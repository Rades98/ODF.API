using Newtonsoft.Json;
using ODF.AppLayer.CQRS.Interfaces.Article;

namespace ODF.API.RequestModels.Forms
{
	public class AddArticleRequestForm : IAddArticle
	{
		[JsonProperty("title", Required = Required.Always)]
		public string Title { get; set; } = string.Empty;

		[JsonProperty("titleTranslationCode", Required = Required.Always)]
		public string TitleTranslationCode { get; set; } = string.Empty;

		[JsonProperty("text", Required = Required.Always)]
		public string Text { get; set; } = string.Empty;

		[JsonProperty("textTranslationCode", Required = Required.Always)]
		public string TextTranslationCode { get; set; } = string.Empty;

		[JsonProperty("pageId", Required = Required.Always)]
		public int PageId { get; set; }

		[JsonProperty("imageUri", Required = Required.AllowNull)]
		public Uri? ImageUri { get; set; }
	}
}
