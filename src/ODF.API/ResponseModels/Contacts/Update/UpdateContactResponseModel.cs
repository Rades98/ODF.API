using ODF.API.ResponseModels.Base;
using ODF.API.ResponseModels.Common.Forms;

namespace ODF.API.ResponseModels.Contacts.Update
{
	public class UpdateContactResponseModel : BaseUpdateResponseModel
	{
		public UpdateContactResponseModel() : base()
		{
		}

		public UpdateContactResponseModel(Form form) : base(form)
		{
		}
	}
}
