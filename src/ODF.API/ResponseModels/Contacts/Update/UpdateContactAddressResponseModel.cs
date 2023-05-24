namespace ODF.API.ResponseModels.Contacts.Update
{
	public class UpdateContactAddressResponseModel : BaseUpdateResponseModel
	{
		public UpdateContactAddressResponseModel(string baseUrl, string countryCode) : base(baseUrl, "/contacts/address", HttpMethods.Post, countryCode)
		{
		}
	}
}
