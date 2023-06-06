using Newtonsoft.Json;
using ODF.API.ResponseModels.Base;

namespace ODF.API.ResponseModels.Lineup
{
	public class LineupResponseModel : BaseResponseModel
	{
		public LineupResponseModel() : base()
		{
		}


		[JsonProperty("lineup", Required = Required.Always)]
		public IDictionary<string, IEnumerable<LineupItemResponseModel>> Lineup { get; set; } = new Dictionary<string, IEnumerable<LineupItemResponseModel>>();
	}
}
