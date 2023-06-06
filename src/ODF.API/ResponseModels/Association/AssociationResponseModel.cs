using Newtonsoft.Json;
using ODF.API.ResponseModels.Base;

namespace ODF.API.ResponseModels.Association
{
	public class AssociationResponseModel : BaseResponseModel
	{
		public AssociationResponseModel(string text, string header) : base()
		{
			Text = text;
			Header = header;
		}

		[JsonProperty("text", Required = Required.Always)]
		public string Text { get; }

		[JsonProperty("header", Required = Required.Always)]
		public string Header { get; }
	}
}
