using Newtonsoft.Json;

namespace ODF.API.ResponseModels.Articles
{
	public class GetArticlesResponseModel : BaseResponseModel
	{
		public GetArticlesResponseModel(string baseUrl, int size, int offset, int pageId, string countryCode) : base(baseUrl, $"/articles?size={size}&offset={offset}&pageId={pageId}", HttpMethods.Get, countryCode)
		{
		}

		[JsonProperty("articles", Required = Required.Always)]
		public IEnumerable<GetArticleResponseModel> Articles { get; set; } = Enumerable.Empty<GetArticleResponseModel>();
	}
}
