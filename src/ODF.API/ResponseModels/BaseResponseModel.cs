using Newtonsoft.Json;
using ODF.API.ResponseModels.Common;
using ODF.API.ResponseModels.Common.Forms;

namespace ODF.API.ResponseModels
{
	public class BaseResponseModel : ApiModel
	{
		private readonly string _baseUrl;

		private readonly List<AppAction> _actions = new();

		public BaseResponseModel(string baseUrl, string relativeUrl, string method, string countryCode, Form? form = null)
		{
			_baseUrl = baseUrl;
			_self = new(_baseUrl + (string.IsNullOrEmpty(countryCode) ? "" : "/" + countryCode) + relativeUrl, "_self", "self", method, form);
		}

		[JsonProperty("_self", Required = Required.Always)]
		public NamedAction _self { get; }

		public List<AppAction> Actions => _actions;

		public void AddAction(string relativeLink, string rel, string method)
			=> _actions.Add(new AppAction(_baseUrl + relativeLink, rel, method));
	}
}
