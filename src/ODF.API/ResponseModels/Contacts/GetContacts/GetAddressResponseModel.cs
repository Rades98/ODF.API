using Newtonsoft.Json;

namespace ODF.API.ResponseModels.Contacts.GetContacts
{
	public class GetAddressResponseModel
	{
		[JsonProperty("street", Required = Required.Always)]
		public string Street { get; set; } = string.Empty;

		[JsonProperty("city", Required = Required.Always)]
		public string City { get; set; } = string.Empty;

		[JsonProperty("postalCode", Required = Required.Always)]
		public string PostalCode { get; set; } = string.Empty;

		[JsonProperty("country", Required = Required.Always)]
		public string Country { get; set; } = string.Empty;
	}
}
