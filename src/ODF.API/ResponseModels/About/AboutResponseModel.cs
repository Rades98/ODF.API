using Newtonsoft.Json;

namespace ODF.API.ResponseModels.About
{
	public class AboutResponseModel : BaseResponseModel
	{
		public AboutResponseModel(string baseUrl, string aboutText, string header, string countryCode) : base(baseUrl, "/about", HttpMethods.Get, countryCode)
		{
			AboutText = aboutText;
			Header = header;
		}

		[JsonProperty("aboutText", Required = Required.Always)]
		public string AboutText { get; }

		[JsonProperty("header", Required = Required.Always)]
		public string Header { get; }
	}
}
