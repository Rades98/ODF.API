namespace ODF.API.ResponseModels.Contacts.Update
{
	public class UpdateContactBankAccResponseModel : BaseUpdateResponseModel
	{
		public UpdateContactBankAccResponseModel(string baseUrl, string countryCode) : base(baseUrl, "/contacts/bankAcc", HttpMethods.Post, countryCode)
		{
		}
	}
}
