using Newtonsoft.Json;
using ODF.API.ResponseModels.Base;

namespace ODF.API.ResponseModels.Donations
{
	public class DonationResponseModel : BaseResponseModel
	{
		public DonationResponseModel() : base()
		{

		}

		[JsonProperty("header", Required = Required.Always)]
		public string Header { get; set; } = string.Empty;

		[JsonProperty("text", Required = Required.Always)]
		public string Text { get; set; } = string.Empty;

		[JsonProperty("bankAccounts", Required = Required.Always)]
		public IEnumerable<DonationBankAccResponseModel> BankAccounts { get; set; } = Enumerable.Empty<DonationBankAccResponseModel>();

		[JsonProperty("additionalMessage", Required = Required.Always)]
		public string AdditionalMessage { get; set; } = string.Empty;
	}
}
