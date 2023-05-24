using Newtonsoft.Json;

namespace ODF.API.RequestModels.Forms.Contacts
{
	public class RemoveBankAccountForm
	{
		[JsonProperty("iban", Required = Required.Always)]
		public string IBAN { get; set; } = string.Empty;
	}
}
