using Newtonsoft.Json;

namespace ODF.API.RequestModels.Forms.Contacts
{
	public class UpdateContactForm
	{
		[JsonProperty("eventName", Required = Required.AllowNull)]
		public string EventName { get; set; } = string.Empty;

		[JsonProperty("eventMAnager", Required = Required.AllowNull)]
		public string EventManager { get; set; } = string.Empty;

		[JsonProperty("email", Required = Required.AllowNull)]
		public string Email { get; set; } = string.Empty;
	}
}
