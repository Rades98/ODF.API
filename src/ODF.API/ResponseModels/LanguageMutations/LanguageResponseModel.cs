using Newtonsoft.Json;
using ODF.API.ResponseModels.Base;

namespace ODF.API.ResponseModels.LanguageMutations
{
	public class LanguageResponseModel : BaseResponseModel
	{
		public LanguageResponseModel(IEnumerable<LanguageModel> languages, string title) : base()
		{
			Title = title;
			Languages = languages;
		}

		[JsonProperty("title", Required = Required.Always)]
		public string Title { get; }

		[JsonProperty("languages", Required = Required.Always)]
		public IEnumerable<LanguageModel> Languages { get; }
	}
}
