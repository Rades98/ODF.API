using Newtonsoft.Json;

namespace ODF.API.ResponseModels.Lineup
{
	public class LineupResponseModel : BaseResponseModel
	{
		public LineupResponseModel(string baseUrl, string countryCode) : base(baseUrl, "/lineup", HttpMethods.Get, countryCode)
		{
		}


		[JsonProperty("lineup", Required = Required.Always)]
		public IDictionary<string, IEnumerable<LineupItemResponseModel>> Lineup { get; set; } = new Dictionary<string, IEnumerable<LineupItemResponseModel>>();
	}
}
