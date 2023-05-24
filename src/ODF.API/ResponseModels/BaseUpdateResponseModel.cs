using Newtonsoft.Json;
using ODF.API.ResponseModels.Common.Forms;

namespace ODF.API.ResponseModels
{
	public class BaseUpdateResponseModel : BaseResponseModel
	{
		public BaseUpdateResponseModel(string baseUrl, string relativeUrl, string method, string countryCode, string? message = null, Form? form = null) : base(baseUrl, relativeUrl, method, countryCode, form)
		{
			if (message is not null)
			{
				Message = message;
			}

			if(form is not null)
			{
				Message = "Nevalidní vstup";
			}
		}

		[JsonProperty("message", Required = Required.Always)]
		public string Message { get; set; } = "Záznam byl úspěšně aktualizován";
	}
}
