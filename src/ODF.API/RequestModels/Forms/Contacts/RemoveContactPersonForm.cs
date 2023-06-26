using Newtonsoft.Json;
using ODF.AppLayer.CQRS.Interfaces.Contact;

namespace ODF.API.RequestModels.Forms.Contacts
{
	public class RemoveContactPersonForm : IRemoveContactPerson
	{
		[JsonProperty("id", Required = Required.Always)]
		public Guid Id { get; set; }
	}
}
