using Newtonsoft.Json;
using ODF.AppLayer.CQRS.Interfaces.User;

namespace ODF.API.RequestModels.Forms.User
{
	public class RegisterUserForm : IRegisterUser
	{
		[JsonProperty("userName", Required = Required.Always)]
		public string UserName { get; set; } = string.Empty;

		[JsonProperty("password", Required = Required.Always)]
		public string Password { get; set; } = string.Empty;

		[JsonProperty("password2", Required = Required.Always)]
		public string Password2 { get; set; } = string.Empty;

		[JsonProperty("email", Required = Required.Always)]
		public string Email { get; set; } = string.Empty;

		[JsonProperty("firstName", Required = Required.Always)]
		public string FirstName { get; set; } = string.Empty;

		[JsonProperty("lastName", Required = Required.AllowNull)]
		public string LastName { get; set; } = string.Empty;
	}
}
