using Newtonsoft.Json;

namespace ODF.API.ResponseModels.LanguageMutations
{
	public class LanguageResponseModel : BaseResponseModel
	{
		public LanguageResponseModel(string baseUrl, IEnumerable<LanguageModel> languages, string title, string countryCode) : base(baseUrl, "/supportedLanguages", HttpMethods.Get, countryCode)
		{
			Title = title;
			Languages = languages;
		}

		[JsonProperty("title", Required = Required.Always)]
		public string Title { get; }

		[JsonProperty("languages", Required = Required.Always)]
		public IEnumerable<LanguageModel> Languages { get; }
	}
}
