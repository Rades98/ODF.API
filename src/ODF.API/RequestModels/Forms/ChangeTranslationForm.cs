using Newtonsoft.Json;

namespace ODF.API.RequestModels.Forms
{
	public class ChangeTranslationForm
	{
		[JsonProperty("translationCode", Required = Required.Always)]
		public string TranslationCode { get; set; } = string.Empty;

		[JsonProperty("text", Required = Required.Always)]
		public string Text { get; set; } = string.Empty;

		[JsonProperty("countryCode", Required = Required.Always)]
		public string CountryCode { get; set; } = string.Empty;
	}
}
