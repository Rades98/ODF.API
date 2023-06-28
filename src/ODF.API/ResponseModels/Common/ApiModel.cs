using Newtonsoft.Json;

namespace ODF.API.ResponseModels.Common
{
	[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
	public class ApiModel
	{
		[JsonConstructor]
		public ApiModel()
		{
		}

		[JsonProperty("_self", Required = Required.Always)]
		public AppAction Self { get; set; } = new("https://odfapi.odf", "_self", "");
	}
}
