using Newtonsoft.Json;
using ODF.API.ResponseModels.Common;

namespace ODF.API.ResponseModels.Lineup.Redaction
{
	public class GetLineupItemRedactionResponseModel : LineupItemResponseModel
	{
		[JsonProperty("updateLineupItem", Required = Required.Always)]
		public NamedAction? UpdateLineupItem { get; set; }

		[JsonProperty("deleteLineupItem", Required = Required.Always)]
		public NamedAction? DeleteLineupItem { get; set; }

		[JsonProperty("userName", Required = Required.Always)]
		public string UserName { get; set; } = string.Empty;

		[JsonProperty("userNote", Required = Required.Always)]
		public string UserNote { get; set; } = string.Empty;
	}
}
