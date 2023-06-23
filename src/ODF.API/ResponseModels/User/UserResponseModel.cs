using Newtonsoft.Json;
using ODF.API.ResponseModels.Base;

namespace ODF.API.ResponseModels.User
{
	public class UserResponseModel : BaseResponseModel
	{
		public UserResponseModel(string userName, string message) : base()
		{
			UserName = userName;
			Message = message;
		}

		[JsonProperty("userName", Required = Required.Always)]
		public string UserName { get; }

		[JsonProperty("message", Required = Required.Always)]
		public string Message { get; }
	}
}
