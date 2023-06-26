using ODF.API.ResponseModels.Base;
using ODF.API.ResponseModels.Common.Forms;

namespace ODF.API.ResponseModels.User
{
	public class UserRegisterResponseModel : BaseCreateResponseModel
	{
		public UserRegisterResponseModel(string message) : base(message)
		{
		}

		public UserRegisterResponseModel(Form form, string message) : base(form, message)
		{
		}

		public UserRegisterResponseModel(Form form) : base(form)
		{
		}
	}
}
