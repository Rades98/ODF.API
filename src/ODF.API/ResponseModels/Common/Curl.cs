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

		public Uri Href { get; }

		public string Rel { get; }

		public string Method { get; }

		public Form? Form { get; set; }
	}
}
