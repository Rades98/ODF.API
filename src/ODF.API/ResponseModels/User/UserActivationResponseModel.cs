using ODF.API.ResponseModels.Base;
using ODF.API.ResponseModels.Common.Forms;

namespace ODF.API.ResponseModels.User
{
	public class UserActivationResponseModel : BaseUpdateResponseModel
	{
		public UserActivationResponseModel(string message) : base(message)
		{

		}

		public UserActivationResponseModel(string message, Form form) : base(form, message)
		{

		}

		public UserActivationResponseModel(Form form) : base(form)
		{

		}
	}
}
