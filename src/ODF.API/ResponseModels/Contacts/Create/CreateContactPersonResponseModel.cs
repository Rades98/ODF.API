using ODF.API.ResponseModels.Base;
using ODF.API.ResponseModels.Common.Forms;

namespace ODF.API.ResponseModels.Contacts.Create
{
	public class CreateContactPersonResponseModel : BaseCreateResponseModel
	{
		public CreateContactPersonResponseModel() : base()
		{
		}

		public CreateContactPersonResponseModel(Form responseForm) : base(responseForm)
		{
		}
	}
}
