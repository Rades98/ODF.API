using ODF.API.ResponseModels.Base;
using ODF.API.ResponseModels.Common.Forms;

namespace ODF.API.ResponseModels.Contacts.Delete
{
	public class DeleteContactBankAccResponseModel : BaseDeleteResponseModel
	{
		public DeleteContactBankAccResponseModel(Form? form = null) : base(form: form)
		{
		}
	}
}
