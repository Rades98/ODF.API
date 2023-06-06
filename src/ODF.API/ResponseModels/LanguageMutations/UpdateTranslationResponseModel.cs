using Newtonsoft.Json;
using ODF.API.ResponseModels.Base;
using ODF.API.ResponseModels.Common.Forms;

namespace ODF.API.ResponseModels.LanguageMutations
{
	public class UpdateTranslationResponseModel : BaseResponseModel
	{
		public UpdateTranslationResponseModel(Form form, string message) : base(form)
		{
			Message = message;
		}

		[JsonProperty("message", Required = Required.Always)]
		public string Message { get; }
	}
}
