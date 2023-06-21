using ODF.API.ResponseModels.Base;
using ODF.API.ResponseModels.Common.Forms;

namespace ODF.API.ResponseModels.Contacts.Update
{
	public class UpdateContactAddressResponseModel : BaseUpdateResponseModel
	{
		public UpdateContactAddressResponseModel() : base()
		{
		}

		public UpdateContactAddressResponseModel(Form form) : base(form)
		{
		}
	}
}
