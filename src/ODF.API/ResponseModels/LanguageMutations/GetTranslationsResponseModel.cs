using Newtonsoft.Json;

namespace ODF.API.ResponseModels.LanguageMutations
{
	public class GetTranslationsResponseModel : BaseResponseModel
	{
		public GetTranslationsResponseModel(string baseUrl, string countryCode, string title, string relativePath) : base(baseUrl, relativePath, HttpMethods.Get, countryCode)
		{
			Title = title;
		}

		[JsonProperty("title", Required = Required.Always)]
		public string Title { get; }

		[JsonProperty("translations", Required = Required.Always)]
		public IEnumerable<GetTranslationResponseModel> Translations { get; set; } = Enumerable.Empty<GetTranslationResponseModel>();
	}
}
