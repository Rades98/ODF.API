using Newtonsoft.Json;
using ODF.API.ResponseModels.Base;

namespace ODF.API.ResponseModels.Articles
{
	public class GetArticleResponseModel : BaseResponseModel
	{
		public GetArticleResponseModel(string title, string body, Uri? imageUrl) : base()
		{
			Title = title;
			Body = body;
			ImageUrl = imageUrl;
		}

		public GetArticleResponseModel(string url, string rel, string method, string title, string body, Uri? imageUrl) : base(url, method, rel)
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
