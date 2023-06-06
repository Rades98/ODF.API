using Newtonsoft.Json;
using ODF.API.ResponseModels.Base;

namespace ODF.API.ResponseModels.About
{
	public class AboutResponseModel : BaseResponseModel
	{
		public AboutResponseModel(string aboutText, string header) : base()
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
