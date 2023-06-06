using Newtonsoft.Json;
using ODF.API.ResponseModels.Base;

namespace ODF.API.ResponseModels.Articles
{
	public class GetArticlesResponseModel : BaseResponseModel
	{
		public GetArticlesResponseModel() : base()
		{
		}

		[JsonProperty("articles", Required = Required.Always)]
		public IEnumerable<GetArticleResponseModel> Articles { get; set; } = Enumerable.Empty<GetArticleResponseModel>();
	}
}
