using Newtonsoft.Json;
using ODF.AppLayer.CQRS.Interfaces.Lineup;

namespace ODF.API.RequestModels.Forms.Lineup
{
	public class AddLineupItemForm : IAddLineupItem
	{
		[JsonProperty("place", Required = Required.Always)]
		public string Place { get; set; } = string.Empty;

		[JsonProperty("interpret", Required = Required.Always)]
		public string Interpret { get; set; } = string.Empty;

		[JsonProperty("performanceName", Required = Required.Always)]
		public string PerformanceName { get; set; } = string.Empty;

		[JsonProperty("description", Required = Required.Always)]
		public string Description { get; set; } = string.Empty;

		[JsonProperty("descriptionTranslationCode", Required = Required.Always)]
		public string DescriptionTranslationCode { get; set; } = string.Empty;

		[JsonProperty("dateTime", Required = Required.Always)]
		public DateTime DateTime { get; set; }

		[JsonProperty("userName", Required = Required.AllowNull)]
		public string? UserName { get; set; }
	}
}
