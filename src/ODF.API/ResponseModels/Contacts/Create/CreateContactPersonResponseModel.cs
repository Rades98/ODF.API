using ODF.API.ResponseModels.Common.Forms;

namespace ODF.API.ResponseModels.Contacts.Create
{
	public class CreateContactPersonResponseModel : BaseCreateResponseModel
	{
		public CreateContactPersonResponseModel(string baseUrl, string countryCode, Form? form = null) : base(baseUrl, "/contacts/person", HttpMethods.Put, countryCode, form: form)
		{
		}
	}
}
