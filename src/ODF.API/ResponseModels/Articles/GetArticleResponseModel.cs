using Newtonsoft.Json;

namespace ODF.API.ResponseModels.Articles
{
	public class GetArticleResponseModel : BaseResponseModel
	{
		public GetArticleResponseModel(string baseUrl, int articleId, string countryCode, string title, string body, Uri? imageUrl) : base(baseUrl, $"/articles/{articleId}", HttpMethods.Get, countryCode)
		{
			Title = title;
			Body = body;
			ImageUrl = imageUrl;
		}

		[JsonProperty("title", Required = Required.Always)]
		public string Title { get; }

		[JsonProperty("body", Required = Required.Always)]
		public string Body { get; }

		[JsonProperty("imageUrl", Required = Required.AllowNull)]
		public Uri? ImageUrl { get; }
	}
}
