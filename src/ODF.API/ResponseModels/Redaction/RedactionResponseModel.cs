﻿using Newtonsoft.Json;
using ODF.API.ResponseModels.Common;

namespace ODF.API.ResponseModels.Redaction
{
	public class RedactionResponseModel : BaseResponseModel
	{
		public RedactionResponseModel(string baseUrl, string countryCode, string title) : base(baseUrl, "/redaction", HttpMethods.Get, countryCode)
		{
			Title = title;
		}

		[JsonProperty("title", Required = Required.Always)]
		public string Title { get; set; }

		[JsonProperty("addAboutArticle", Required = Required.Always)]
		public NamedAction? AddAboutArticle { get; set; }

		[JsonProperty("addAssociationArticle", Required = Required.Always)]
		public NamedAction? AddAssociationArticle { get; set; }
	}
}
