using Newtonsoft.Json;
using ODF.API.ResponseModels.Common.Forms;

namespace ODF.API.ResponseModels.Common
{
	[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
	public class AppAction
	{
		public AppAction(string href, string rel, string method, Form? form = null)
		{
			Curl ??= new Curl(new(string.IsNullOrEmpty(href) ? "http://api.odf" : href), rel, method, form);
		}

		[JsonProperty("curl")]
		public Curl Curl { get; set; }
	}
}
