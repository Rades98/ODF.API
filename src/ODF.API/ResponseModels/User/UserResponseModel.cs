using Newtonsoft.Json;
using ODF.API.ResponseModels.Base;
using ODF.API.ResponseModels.Common.Forms;

namespace ODF.API.ResponseModels.User
{
	public class UserResponseModel : BaseResponseModel
	{
		public UserResponseModel(string userName, Form form) : base(form)
		{
			UserName = userName;
		}

		[JsonProperty("userName", Required = Required.Always)]
		public string UserName { get; }
	}
}
