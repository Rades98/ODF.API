using Newtonsoft.Json;
using ODF.API.ResponseModels.Common.Forms;

namespace ODF.API.ResponseModels.LanguageMutations
{
	public class PostTranslationResponseModel : BaseResponseModel
	{
		public PostTranslationResponseModel(string baseUrl, string countryCode, Form form, string message) : base(baseUrl, "/translations", HttpMethods.Post, countryCode)
		{
			_self.Curl.Form = form;
			Message = message;
		}

		[JsonProperty("message", Required = Required.Always)]
		public string Message { get; }
	}
}
