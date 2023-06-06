using Newtonsoft.Json;
using ODF.API.ResponseModels.Base;
using ODF.API.ResponseModels.Common;
using ODF.API.ResponseModels.Common.Forms;

namespace ODF.API.ResponseModels.Articles
{
	public class CreateArticleResponseModel : BaseResponseModel
	{
		public CreateArticleResponseModel(string message, Form form) : base(form)
		{
			Message = message;
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
