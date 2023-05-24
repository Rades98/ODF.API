using Newtonsoft.Json;

namespace ODF.API.RequestModels.Forms.Contacts
{
	public class RemoveContactPersonForm
	{
		[JsonProperty("id", Required = Required.Always)]
		public Guid Id { get; set; }
	}
}
