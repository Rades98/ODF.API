using ODF.API.ResponseModels.Base;
using ODF.API.ResponseModels.Common.Forms;

namespace ODF.API.ResponseModels.Contacts.Create
{
	public class CreateContactBankAccResponseModel : BaseCreateResponseModel
	{
		public CreateContactBankAccResponseModel() : base()
		{
		}

		public CreateContactBankAccResponseModel(Form form) : base(form)
		{
		}
	}
}
