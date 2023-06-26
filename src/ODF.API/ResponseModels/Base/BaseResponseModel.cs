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

		public BaseResponseModel(Form form)
		{
			_self.Curl.Form = form;
			Actions ??= new();
		}

		public BaseResponseModel(string url, string method, string rel, Form? form = null)
		{
			_self = new(url, rel, method, form);
			Actions ??= new();
		}

		[JsonProperty(nameof(_self), Required = Required.Always)]
		public AppAction _self { get; set; } = new("https://odfapi.odf", "_self", "");

		[JsonProperty("actions")]
		public List<AppAction> Actions { get; set; }

		public void AddAction(AppAction action)
			=> Actions.Add(action);
	}
}
