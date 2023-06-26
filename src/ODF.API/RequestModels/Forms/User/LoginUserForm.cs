using Newtonsoft.Json;
using ODF.AppLayer.CQRS.Interfaces.User;

namespace ODF.API.RequestModels.Forms.User
{
	public class LoginUserForm : ILoginUser
	{
		[JsonProperty("userName", Required = Required.Always)]
		public string UserName { get; set; } = string.Empty;

		[JsonProperty("password", Required = Required.Always)]
		public string Password { get; set; } = string.Empty;
	}
}
