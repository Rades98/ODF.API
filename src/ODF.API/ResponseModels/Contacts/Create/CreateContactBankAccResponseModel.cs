using ODF.API.ResponseModels.Common.Forms;

namespace ODF.API.ResponseModels.Contacts.Create
{
	public class CreateContactBankAccResponseModel : BaseCreateResponseModel
	{
		public CreateContactBankAccResponseModel(string baseUrl, string countryCode, Form? form = null) : base(baseUrl, "/contacts/bankAcc", HttpMethods.Put, countryCode, form: form)
		{
		}
	}
}
