using Newtonsoft.Json;

namespace ODF.API.ResponseModels.Contacts.GetContacts
{
	public class ContactResponseModel : BaseResponseModel
	{
		public ContactResponseModel(string baseUrl, string countryCode) : base(baseUrl, "/contacts", HttpMethods.Get, countryCode)
		{
		}

		[JsonProperty("eventName", Required = Required.Always)]
		public string EventName { get; set; } = string.Empty;

		[JsonProperty("eventManager", Required = Required.Always)]
		public string EventManager { get; set; } = string.Empty;

		[JsonProperty("eventManagerTranslation", Required = Required.Always)]
		public string EventManagerTranslation { get; set; } = string.Empty;

		[JsonProperty("email", Required = Required.Always)]
		public string Email { get; set; } = string.Empty;

		[JsonProperty("emailTranslation", Required = Required.Always)]
		public string EmailTranslation { get; set; } = string.Empty;

		[JsonProperty("address", Required = Required.Always)]
		public GetAddressResponseModel Address { get; set; } = new();

		[JsonProperty("bankAccounts", Required = Required.Always)]
		public IEnumerable<GetBankAccountResponseModel> BankAccounts { get; set; } = new List<GetBankAccountResponseModel>();

		[JsonProperty("contactPersons", Required = Required.Always)]
		public IEnumerable<GetContactPersonResponseModel> ContactPersons { get; set; } = new List<GetContactPersonResponseModel>();
	}
}
