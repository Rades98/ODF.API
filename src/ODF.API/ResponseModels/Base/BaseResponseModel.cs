using Newtonsoft.Json;
using ODF.API.ResponseModels.Common;
using ODF.API.ResponseModels.Common.Forms;

namespace ODF.API.ResponseModels.Base
{
	public class BaseResponseModel : ApiModel, IBaseResponseModel
	{
		[JsonConstructor]
		public BaseResponseModel()
		{
			Actions ??= new();
		}

		public BaseResponseModel(Form? form = null)
		{
			_self.Curl.Form = form;
		}

		public BaseResponseModel(string url, string method, string rel, Form? form = null)
		{
			_self = new(url, "_self", rel, method, form);
		}

		[JsonProperty("_self", Required = Required.Always)]
		public NamedAction _self { get; set; } = new("http://odfapi.odf", "", "", "");

		[JsonProperty("actions")]
		public List<AppAction> Actions { get; set; }

		public void AddAction(AppAction action)
			=> Actions.Add(action);
	}
}
