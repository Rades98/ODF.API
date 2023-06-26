using Newtonsoft.Json;
using ODF.AppLayer.CQRS.Interfaces.User;

namespace ODF.API.RequestModels.Forms.User
{
	public class ActivateUserForm : IActivateUser
	{
		[JsonProperty("hash", Required = Required.Always)]
		public string Hash { get; set; } = string.Empty;
	}
}
