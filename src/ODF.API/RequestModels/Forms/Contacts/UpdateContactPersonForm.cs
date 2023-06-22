using Newtonsoft.Json;

namespace ODF.API.RequestModels.Forms.Contacts
{
	public class UpdateContactPersonForm
	{
		[JsonProperty("email", Required = Required.AllowNull)]
		public string Email { get; set; } = string.Empty;

		[JsonProperty("title", Required = Required.AllowNull)]
		public string Title { get; set; } = string.Empty;

		[JsonProperty("name", Required = Required.AllowNull)]
		public string Name { get; set; } = string.Empty;

		[JsonProperty("surname", Required = Required.AllowNull)]
		public string Surname { get; set; } = string.Empty;

		[JsonProperty("roles", Required = Required.AllowNull)]
		public IEnumerable<string> Roles { get; set; } = new List<string>();

		[JsonProperty("base64Image", Required = Required.AllowNull)]
		public string Base64Image { get; set; } = string.Empty;

		[JsonProperty("id", Required = Required.AllowNull)]
		public Guid Id { get; set; }

		[JsonProperty("order", Required = Required.AllowNull)]
		public int Order { get; set; }
	}
}
