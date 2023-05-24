using Newtonsoft.Json;

namespace ODF.API.ResponseModels.Contacts.GetContacts
{
	public class GetContactPersonResponseModel
	{
		[JsonProperty("email", Required = Required.Always)]
		public string? Email { get; set; }

		[JsonProperty("title", Required = Required.Always)]
		public string? Title { get; set; }

		[JsonProperty("name", Required = Required.Always)]
		public string? Name { get; set; }

		[JsonProperty("surname", Required = Required.Always)]
		public string? Surname { get; set; }

		[JsonProperty("roles", Required = Required.Always)]
		public IEnumerable<string> Roles { get; set; } = new List<string>();

		[JsonProperty("base64Image", Required = Required.Always)]
		public string? Base64Image { get; set; } = null;
	}
}
