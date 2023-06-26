using Newtonsoft.Json;
using ODF.AppLayer.CQRS.Interfaces.Contact;

namespace ODF.API.RequestModels.Forms.Contacts
{
	public class RemoveBankAccountForm : IRemoveBankAccount
	{
		[JsonProperty("iban", Required = Required.Always)]
		public string IBAN { get; set; } = string.Empty;
	}
}
