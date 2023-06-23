using Newtonsoft.Json;
using ODF.API.ResponseModels.Common;

namespace ODF.API.ResponseModels.Contacts.Redaction
{
	public class GetContactPersonRedactionResponseModel
	{
		[JsonProperty("title", Required = Required.Always)]
		public string? Title { get; set; }

		[JsonProperty("name", Required = Required.Always)]
		public string? Name { get; set; }

		[JsonProperty("surname", Required = Required.Always)]
		public string? Surname { get; set; }

		[JsonProperty("email", Required = Required.Always)]
		public string? Email { get; set; }

		[JsonProperty("updateContactPerson", Required = Required.Always)]
		public NamedAction? UpdateContactPerson { get; set; }

		[JsonProperty("deleteContactPerson", Required = Required.Always)]
		public NamedAction? DeleteContactPerson { get; set; }
	}
}
