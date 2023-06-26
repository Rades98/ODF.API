using Newtonsoft.Json;
using ODF.AppLayer.CQRS.Interfaces.Contact;

namespace ODF.API.RequestModels.Forms.Contacts
{
	public class UpdateContactForm : IUpdateContact
	{
		[JsonProperty("eventName", Required = Required.AllowNull)]
		public string EventName { get; set; } = string.Empty;

		[JsonProperty("eventMAnager", Required = Required.AllowNull)]
		public string EventManager { get; set; } = string.Empty;

		[JsonProperty("email", Required = Required.AllowNull)]
		public string Email { get; set; } = string.Empty;
	}
}
