using Newtonsoft.Json;

namespace ODF.API.ResponseModels
{
	public class BaseDeleteResponseModel : BaseResponseModel
	{
		public BaseDeleteResponseModel(string baseUrl, string relativeUrl, string method, string countryCode, string? message = null) : base(baseUrl, relativeUrl, method, countryCode)
		{
			if (message is not null)
			{
				Message = message;
			}
		}

		[JsonProperty("message", Required = Required.Always)]
		public string Message { get; set; } = "Záznam byl úspěšně smazán";
	}
}
