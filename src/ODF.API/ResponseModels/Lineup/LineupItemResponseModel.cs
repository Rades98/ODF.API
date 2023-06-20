using Newtonsoft.Json;

namespace ODF.API.ResponseModels.Lineup
{
	public class LineupItemResponseModel
	{
		[JsonProperty("date", Required = Required.Always)]
		public string Date { get; set; } = string.Empty;

		[JsonProperty("time", Required = Required.Always)]
		public string Time { get; set; } = string.Empty;

		[JsonProperty("interpret", Required = Required.Always)]
		public string Interpret { get; set; } = string.Empty;

		[JsonProperty("performanceName", Required = Required.Always)]
		public string PerformanceName { get; set; } = string.Empty;

		[JsonProperty("description", Required = Required.Always)]
		public string Description { get; set; } = string.Empty;
	}
}
