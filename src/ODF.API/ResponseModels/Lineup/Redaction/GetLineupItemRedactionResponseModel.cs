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
	}
}
