using Newtonsoft.Json;

namespace ODF.API.RequestModels.Forms.Contacts
{
	public class UpdateAddressForm
	{
		[JsonProperty("street", Required = Required.AllowNull)]
		public string Street { get; set; } = string.Empty;

		[JsonProperty("city", Required = Required.AllowNull)]
		public string City { get; set; } = string.Empty;

		[JsonProperty("postalCode", Required = Required.AllowNull)]
		public string PostalCode { get; set; } = string.Empty;

		[JsonProperty("country", Required = Required.AllowNull)]
		public string Country { get; set; } = string.Empty;
	}
}
