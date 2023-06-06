using Newtonsoft.Json;
using ODF.API.ResponseModels.Base;

namespace ODF.API.ResponseModels.LanguageMutations
{
	public class GetTranslationsResponseModel : BaseResponseModel
	{
		public GetTranslationsResponseModel(string title) : base()
		{
			Title = title;
		}

		[JsonProperty("title", Required = Required.Always)]
		public string Title { get; }

		[JsonProperty("translations", Required = Required.Always)]
		public IEnumerable<GetTranslationResponseModel> Translations { get; set; } = Enumerable.Empty<GetTranslationResponseModel>();
	}
}
