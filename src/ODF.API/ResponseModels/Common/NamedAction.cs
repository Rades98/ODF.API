using Newtonsoft.Json;
using ODF.API.ResponseModels.Common.Forms;

namespace ODF.API.ResponseModels.Common
{
	public class NamedAction : AppAction
	{
		public NamedAction(string href, string name, string rel, string method, Form? form = null) : base(href, rel, method, form)
		{
			ActionName = name;
		}

		[JsonProperty("actionName", Required = Required.Always)]
		public string ActionName { get; internal set; }
	}

}
