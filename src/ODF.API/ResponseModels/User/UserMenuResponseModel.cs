using Newtonsoft.Json;
using ODF.API.ResponseModels.Base;
using ODF.API.ResponseModels.Common;

namespace ODF.API.ResponseModels.User
{
	public class UserMenuResponseModel : BaseResponseModel
	{
		[JsonProperty("lineup", Required = Required.Always)]
		public NamedAction? UserLineupAction { get; set; }

		[JsonProperty("chat", Required = Required.Always)]
		public NamedAction? UserChatAction { get; set; }

		[JsonProperty("subscription", Required = Required.Always)]
		public NamedAction? UserSubscriptionAction { get; set; }
	}
}
