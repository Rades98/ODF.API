using Newtonsoft.Json;

namespace ODF.API.RequestModels.Forms.Lineup
{
	public class DeleteLineupItemForm
	{
		[JsonProperty("id", Required = Required.Always)]
		public Guid Id { get; set; }
	}
}
