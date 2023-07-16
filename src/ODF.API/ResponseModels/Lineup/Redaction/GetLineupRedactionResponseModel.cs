using Newtonsoft.Json;
using ODF.API.ResponseModels.Base;
using ODF.API.ResponseModels.Common;

namespace ODF.API.ResponseModels.Lineup.Redaction
{
	public class GetLineupRedactionResponseModel : BaseResponseModel
	{
		public GetLineupRedactionResponseModel() : base()
		{

		}

		[JsonProperty("addLineupItem", Required = Required.Always)]
		public NamedAction? AddLineupItem { get; set; }

		[JsonProperty("lineupItems", Required = Required.Always)]
		public IEnumerable<GetLineupItemRedactionResponseModel> LineupItems { get; set; } = new List<GetLineupItemRedactionResponseModel>();

		[JsonProperty("redactionPartName", Required = Required.Always)]
		public string RedactionPartName { get; set; } = string.Empty;
	}
}
