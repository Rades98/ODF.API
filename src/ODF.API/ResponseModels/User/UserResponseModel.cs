using Newtonsoft.Json;
using ODF.API.ResponseModels.Base;

namespace ODF.API.ResponseModels.User
{
	public class UserResponseModel : BaseResponseModel
	{
		public UserResponseModel(string userName) : base()
		{
			UserName = userName;
		}

		[JsonProperty("userName", Required = Required.Always)]
		public string UserName { get; }
	}
}
