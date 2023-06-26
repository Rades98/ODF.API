using Newtonsoft.Json;
using ODF.AppLayer.CQRS.Interfaces.Lineup;

namespace ODF.API.RequestModels.Forms.Lineup
{
	public class UpdateLineupItemForm : IUpdateLineupItem
	{
		[JsonProperty("id", Required = Required.Always)]
		public Guid Id { get; set; }

		[JsonProperty("place", Required = Required.AllowNull)]
		public string Place { get; set; } = string.Empty;

		[JsonProperty("interpret", Required = Required.AllowNull)]
		public string Interpret { get; set; } = string.Empty;

		[JsonProperty("performanceName", Required = Required.AllowNull)]
		public string PerformanceName { get; set; } = string.Empty;

		[JsonProperty("description", Required = Required.AllowNull)]
		public string Description { get; set; } = string.Empty;

		[JsonProperty("descriptionTranslationCode", Required = Required.Always)]
		public string DescriptionTranslationCode { get; set; } = string.Empty;

		[JsonProperty("dateTime", Required = Required.AllowNull)]
		public DateTime? DateTime { get; set; }

		[JsonProperty("userName", Required = Required.AllowNull)]
		public string? UserName { get; set; }
	}
}
