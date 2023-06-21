using ODF.API.ResponseModels.Base;
using ODF.API.ResponseModels.Common.Forms;

namespace ODF.API.ResponseModels.Contacts.Delete
{
	public class DeleteContactPersonResponseModel : BaseDeleteResponseModel
	{
		public DeleteContactPersonResponseModel() : base()
		{
		}

		public DeleteContactPersonResponseModel(Form form) : base(form)
		{
		}
	}
}
