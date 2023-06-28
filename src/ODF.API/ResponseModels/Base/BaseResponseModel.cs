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
			Self.Curl.Form = form;
			Actions ??= new();
		}

		public BaseResponseModel(string url, string method, string rel, Form? form = null)
		{
			Self = new(url, rel, method, form);
			Actions ??= new();
		}

		[JsonProperty("actions")]
		public List<AppAction> Actions { get; set; }

		public void AddAction(AppAction action)
			=> Actions.Add(action);
	}
}
