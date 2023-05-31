using Newtonsoft.Json;
using ODF.API.ResponseModels.Common;

namespace ODF.API.ResponseModels.LanguageMutations
{
	public class GetTranslationResponseModel : BaseResponseModel
	{
		public GetTranslationResponseModel(string baseUrl, string countryCode, string translationCode, string text) : base(baseUrl, "/translations", HttpMethods.Get, countryCode)
		{
			TranslationCode = translationCode;
			Text = text;
		}

		[JsonProperty("translationCode", Required = Required.Always)]
		public string TranslationCode { get; }

		[JsonProperty("text", Required = Required.Always)]
		public string Text { get; }

		[JsonProperty("changeDeTranslation", Required = Required.Always)]
		public NamedAction? ChangeDeTranslation { get; set; }

		[JsonProperty("changeTranslation", Required = Required.Always)]
		public NamedAction? ChangeTranslation { get; set; }

		[JsonProperty("changeEnTranslation", Required = Required.Always)]
		public NamedAction? ChangeEnTranslation { get; set; }
	}
}
