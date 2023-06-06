using Newtonsoft.Json;
using ODF.API.ResponseModels.Base;
using ODF.API.ResponseModels.Common;

namespace ODF.API.ResponseModels.Redaction
{
	public class RedactionResponseModel : BaseResponseModel
	{
		public RedactionResponseModel(string title) : base()
		{
			Title = title;
		}

		[JsonProperty("title", Required = Required.Always)]
		public string Title { get; set; }

		[JsonProperty("addAboutArticle", Required = Required.Always)]
		public NamedAction? AddAboutArticle { get; set; }

		[JsonProperty("addAssociationArticle", Required = Required.Always)]
		public NamedAction? AddAssociationArticle { get; set; }

		[JsonProperty("addLineupItem", Required = Required.Always)]
		public NamedAction? AddLineupItem { get; set; }

		[JsonProperty("updateContacts", Required = Required.Always)]
		public NamedAction? UpdateContacts { get; set; }
	}
}
