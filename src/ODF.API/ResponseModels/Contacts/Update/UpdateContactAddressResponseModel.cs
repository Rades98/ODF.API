using ODF.API.ResponseModels.Base;
using ODF.API.ResponseModels.Common.Forms;

namespace ODF.API.ResponseModels.Contacts.Update
{
	public class UpdateContactAddressResponseModel : BaseUpdateResponseModel
	{
		public UpdateContactAddressResponseModel(Form? form = null) : base(form: form)
		{
		}
	}
}
