using Newtonsoft.Json;

namespace ODF.API.RequestModels.Forms
{
	public class UserRequestForm
	{
		[JsonProperty("userName", Required = Required.Always)]
		public string UserName { get; set; } = string.Empty;

		[JsonProperty("password", Required = Required.Always)]
		public string Password { get; set; } = string.Empty;
	}
}
