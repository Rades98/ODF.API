namespace ODF.API.ResponseModels.Contacts.Update
{
	public class UpdateContactPersonResponseModel : BaseUpdateResponseModel
	{
		public UpdateContactPersonResponseModel(string baseUrl, string countryCode) : base(baseUrl, "updateContactPerson", HttpMethods.Post, countryCode)
		{
		}
	}
}
