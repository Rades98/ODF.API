using Newtonsoft.Json;
using ODF.API.ResponseModels.Lineup;

namespace ODF.API.ResponseModels.User
{
	public class UserLineupItemResponseModel : LineupItemResponseModel
	{
		[JsonProperty("userNote", Required = Required.Always)]
		public string UserNote { get; set; } = string.Empty;
	}
}
