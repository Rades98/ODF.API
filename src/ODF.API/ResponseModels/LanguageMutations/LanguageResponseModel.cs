using Newtonsoft.Json;

namespace ODF.API.ResponseModels.LanguageMutations
{
	public class LanguageResponseModel : BaseResponseModel
	{
		public LanguageResponseModel(string baseUrl, IEnumerable<LanguageModel> languages, string title, string countryCode) : base(baseUrl, "/supportedLanguages", HttpMethods.Get, countryCode)
		{
			Titile = title;
			Languages = languages;
		}

		[JsonProperty("title", Required = Required.Always)]
		public string Titile { get; }

		[JsonProperty("languages", Required = Required.Always)]
		public IEnumerable<LanguageModel> Languages { get; }
	}
}
