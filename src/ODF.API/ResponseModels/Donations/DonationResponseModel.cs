using Newtonsoft.Json;
using ODF.API.ResponseModels.Base;

namespace ODF.API.ResponseModels.Donations
{
	public class DonationResponseModel : BaseResponseModel
	{
		public DonationResponseModel(IEnumerable<string> qrStrings, string header, string text) : base()
		{
			QrStrings = qrStrings;
			Header = header;
			Text = text;
		}

		[JsonProperty("header", Required = Required.Always)]
		public string Header { get; }

		[JsonProperty("text", Required = Required.Always)]
		public string Text { get; }

		[JsonProperty("qrString", Required = Required.Always)]
		public IEnumerable<string> QrStrings { get; }
	}
}
