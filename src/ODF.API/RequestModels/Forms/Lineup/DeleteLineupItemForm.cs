using Newtonsoft.Json;
using ODF.AppLayer.CQRS.Interfaces.Lineup;

namespace ODF.API.RequestModels.Forms.Lineup
{
	public class DeleteLineupItemForm : IDeleteLineupItem
	{
		[JsonProperty("id", Required = Required.Always)]
		public Guid Id { get; set; }
	}
}
