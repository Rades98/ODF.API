using Newtonsoft.Json;
using ODF.API.ResponseModels.Base;
using ODF.API.ResponseModels.Common;

namespace ODF.API.ResponseModels.LanguageMutations
{
	public class LanguageModel : BaseResponseModel
	{
		public LanguageModel(string name, string countryCode)
		{
			Name = name;
			CountryCode = countryCode;
		}

		[JsonProperty("name", Required = Required.Always)]
		public string Name { get; }

		[JsonProperty("countryCode", Required = Required.Always)]
		public string CountryCode { get; }

		[JsonProperty("changeLanguage", Required = Required.AllowNull)]
		public NamedAction? ChangeLanguage { get; set; }
	}
}
