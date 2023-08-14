using Newtonsoft.Json;
using ODF.API.ResponseModels.Base;

namespace ODF.API.ResponseModels.Donations
{
	public class DonationResponseModel : BaseResponseModel
	{
		public DonationResponseModel(IEnumerable<DonationBankAccResponseModel> bankAccs, string header, string text, string additionalMessage) : base()
		{
			BankAccounts = bankAccs;
			Header = header;
			Text = text;
			AdditionalMessage = additionalMessage;
		}

		[JsonProperty("header", Required = Required.Always)]
		public string Header { get; }

		[JsonProperty("text", Required = Required.Always)]
		public string Text { get; }

		[JsonProperty("bankAccounts", Required = Required.Always)]
		public IEnumerable<DonationBankAccResponseModel> BankAccounts { get; } = Enumerable.Empty<DonationBankAccResponseModel>();

		[JsonProperty("additionalMessage", Required = Required.Always)]
		public string AdditionalMessage { get; }
	}
}
