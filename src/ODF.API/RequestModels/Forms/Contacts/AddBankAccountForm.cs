using Newtonsoft.Json;
using ODF.AppLayer.CQRS.Interfaces.Contact;

namespace ODF.API.RequestModels.Forms.Contacts
{
	public class AddBankAccountForm : IAddBankAccount
	{
		[JsonProperty("bank", Required = Required.Always)]
		public string Bank { get; set; } = string.Empty;

		[JsonProperty("accountId", Required = Required.Always)]
		public string AccountId { get; set; } = string.Empty;

		[JsonProperty("iban", Required = Required.Always)]
		public string IBAN { get; set; } = string.Empty;
	}
}
