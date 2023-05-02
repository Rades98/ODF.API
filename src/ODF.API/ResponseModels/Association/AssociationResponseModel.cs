using Newtonsoft.Json;

namespace ODF.API.ResponseModels.Association
{
	public class AssociationResponseModel : BaseResponseModel
	{
		public AssociationResponseModel(string baseUrl, string text, string header, string countryCode) : base(baseUrl, "/association", HttpMethods.Get, countryCode)
		{
			Text = text;
			Header = header;
		}

		[JsonProperty("text", Required = Required.Always)]
		public string Text { get; }

		[JsonProperty("header", Required = Required.Always)]
		public string Header { get; }
	}
}
