using Newtonsoft.Json;

namespace ODF.API.ResponseModels.Contacts.Redaction
{
	public class GetContactPersonsRedactionResponseModel
	{
		[JsonProperty("title", Required = Required.Always)]
		public string? Title { get; set; }

		[JsonProperty("contactPersons", Required = Required.Always)]
		public IEnumerable<GetContactPersonRedactionResponseModel> ContactPersons { get; set; } = Enumerable.Empty<GetContactPersonRedactionResponseModel>();
	}
}
