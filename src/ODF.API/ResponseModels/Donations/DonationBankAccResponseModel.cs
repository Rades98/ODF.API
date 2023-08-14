using Newtonsoft.Json;

namespace ODF.API.ResponseModels.Donations
{
	public class DonationBankAccResponseModel
	{
		[JsonProperty("qrString", Required = Required.Always)]
		public string QrString { get; set; } = string.Empty;

		[JsonProperty("accountId", Required = Required.Always)]
		public string AccountId { get; set; } = string.Empty;

		[JsonProperty("accountIdTranslation", Required = Required.Always)]
		public string AccountIdTranslation { get; set; } = string.Empty;

		[JsonProperty("iban", Required = Required.Always)]
		public string IBAN { get; set; } = string.Empty;

		[JsonProperty("ibanTranslation", Required = Required.Always)]
		public string IBANTranslation { get; set; } = string.Empty;
	}
}
