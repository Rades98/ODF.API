namespace ODF.API.ResponseModels.Contacts.Delete
{
	public class DeleteContactBankAccResponseModel : BaseDeleteResponseModel
	{
		public DeleteContactBankAccResponseModel(string baseUrl, string countryCode) : base(baseUrl, "/contacts/bankAcc", HttpMethods.Delete, countryCode)
		{
		}
	}
}
