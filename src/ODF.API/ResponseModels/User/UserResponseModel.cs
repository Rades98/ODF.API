using Newtonsoft.Json;
using ODF.API.ResponseModels.Common.Forms;

namespace ODF.API.ResponseModels.User
{
	public class UserResponseModel : BaseResponseModel
	{
		public UserResponseModel(string baseUrl, string userName, string token, string countryCode, Form form) : base(baseUrl, "/user", HttpMethods.Post, countryCode)
		{
			UserName = userName;
			Token = token;

			_self.Curl.Form = form;
		}

		[JsonProperty("userName", Required = Required.Always)]
		public string UserName { get; }

		[JsonProperty("token", Required = Required.Always)]
		public string Token { get; }
	}
}
