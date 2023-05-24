using Newtonsoft.Json;

namespace ODF.API.RequestModels.Forms.Contacts
{
	public class AddContactPersonForm
	{
		[JsonProperty("email", Required = Required.Always)]
		public string Email { get; set; } = string.Empty;

		[JsonProperty("title", Required = Required.Always)]
		public string Title { get; set; } = string.Empty;

		[JsonProperty("name", Required = Required.Always)]
		public string Name { get; set; } = string.Empty;

		[JsonProperty("surname", Required = Required.Always)]
		public string Surname { get; set; } = string.Empty;

		[JsonProperty("role", Required = Required.Always)]
		public IEnumerable<string> Roles { get; set; } = new List<string>();

		[JsonProperty("base64Image", Required = Required.Always)]
		public string Base64Image { get; set; } = string.Empty;
	}
}
