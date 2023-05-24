namespace ODF.API.ResponseModels.Contacts.Update
{
	public class UpdateContactResponseModel : BaseUpdateResponseModel
	{
		public UpdateContactResponseModel(string baseUrl, string countryCode) : base(baseUrl, "/contacts", HttpMethods.Post, countryCode)
		{
		}
	}
}
