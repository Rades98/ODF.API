namespace ODF.API.ResponseModels.Contacts.Delete
{
	public class DeleteContactPersonResponseModel : BaseDeleteResponseModel
	{
		public DeleteContactPersonResponseModel(string baseUrl, string countryCode) : base(baseUrl, "/contacts/person", HttpMethods.Delete, countryCode)
		{
		}
	}
}
