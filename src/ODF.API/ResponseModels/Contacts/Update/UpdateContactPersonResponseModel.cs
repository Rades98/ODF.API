using ODF.API.ResponseModels.Base;
using ODF.API.ResponseModels.Common.Forms;

namespace ODF.API.ResponseModels.Contacts.Update
{
	public class UpdateContactPersonResponseModel : BaseUpdateResponseModel
	{
		public UpdateContactPersonResponseModel() : base()
		{
		}

		public UpdateContactPersonResponseModel(Form form) : base(form)
		{
		}
	}
}
