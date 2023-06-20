using ODF.API.ResponseModels.Base;
using ODF.API.ResponseModels.Common.Forms;

namespace ODF.API.ResponseModels.Contacts.Update
{
	public class UpdateContactPersonResponseModel : BaseUpdateResponseModel
	{
		public UpdateContactPersonResponseModel(Form? form = null) : base(form: form)
		{
		}
	}
}
