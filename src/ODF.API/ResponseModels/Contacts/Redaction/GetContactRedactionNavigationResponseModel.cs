using Newtonsoft.Json;
using ODF.API.ResponseModels.Base;
using ODF.API.ResponseModels.Common;

namespace ODF.API.ResponseModels.Contacts.Redaction
{
	public class GetContactRedactionNavigationResponseModel : BaseResponseModel
	{
		public GetContactRedactionNavigationResponseModel() : base()
		{
		}

		[JsonProperty("updateContacts", Required = Required.Always)]
		public NamedAction? UpdateContacts { get; set; }

		[JsonProperty("updateAddress", Required = Required.Always)]
		public NamedAction? UpdateAddress { get; set; }

		[JsonProperty("addBankAccount", Required = Required.Always)]
		public NamedAction? AddBankAccount { get; set; }

		[JsonProperty("removeBankAccountActions", Required = Required.Always)]
		public IEnumerable<NamedAction> RemoveBankAccountActions { get; set; } = new List<NamedAction>();

		[JsonProperty("addContactPerson", Required = Required.Always)]
		public NamedAction? AddContactPerson { get; set; }

		[JsonProperty("contactPerson", Required = Required.Always)]
		public IEnumerable<GetContactPersonRedactionResponseModel> ContactPersons { get; set; } = new List<GetContactPersonRedactionResponseModel>();
	}
}
