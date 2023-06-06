using Newtonsoft.Json;
using ODF.API.ResponseModels.Common.Forms;

namespace ODF.API.ResponseModels.Common
{
	[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
	public class Curl
	{
		public Curl(Uri href, string rel, string method, Form? form = null)
		{
			Href = href;
			Rel = rel;
			Method = method;
			Form = form;
		}

		[JsonProperty("href")]
		public Uri Href { get; internal set; }

		[JsonProperty("rel")]
		public string Rel { get; internal set; }

		[JsonProperty("method")]
		public string Method { get; internal set; }

		[JsonProperty("form")]
		public Form? Form { get; internal set; }
	}
}
