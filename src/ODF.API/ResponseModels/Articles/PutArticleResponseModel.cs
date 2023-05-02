using Newtonsoft.Json;
using ODF.API.ResponseModels.Common;
using ODF.API.ResponseModels.Common.Forms;

namespace ODF.API.ResponseModels.Articles
{
	public class PutArticleResponseModel : BaseResponseModel
	{
		public PutArticleResponseModel(string baseUrl, string message, Form form) : base(baseUrl, "/articles", HttpMethods.Put, "cz")
		{
			Message = message;
			_self.Curl.Form = form;
		}

		[JsonProperty("message", Required = Required.Always)]
		public string Message { get; }

		[JsonProperty("addTitleDeTranslation", Required = Required.Always)]
		public NamedAction? AddTitleDeTranslation { get; set; }

		[JsonProperty("addTextDeTranslation", Required = Required.Always)]
		public NamedAction? AddTextDeTranslation { get; set; }

		[JsonProperty("addTitleEnTranslation", Required = Required.Always)]
		public NamedAction? AddTitleEnTranslation { get; set; }

		[JsonProperty("addTextEnTranslation", Required = Required.Always)]
		public NamedAction? AddTextEnTranslation { get; set; }
	}
}
