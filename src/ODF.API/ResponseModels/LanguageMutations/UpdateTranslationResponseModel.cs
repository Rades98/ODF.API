using Newtonsoft.Json;
using ODF.API.ResponseModels.Common.Forms;

namespace ODF.API.ResponseModels.LanguageMutations
{
	public class UpdateTranslationResponseModel : BaseResponseModel
	{
		public UpdateTranslationResponseModel(string baseUrl, string countryCode, Form form, string message) : base(baseUrl, "/translations", HttpMethods.Post, countryCode)
		{
			_self.Curl.Form = form;
			Message = message;
		}

		[JsonProperty("message", Required = Required.Always)]
		public string Message { get; }
	}
}
