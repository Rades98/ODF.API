using Newtonsoft.Json;
using ODF.API.ResponseModels.Common.Forms;

namespace ODF.API.ResponseModels.Base
{
	public class BaseDeleteResponseModel : BaseResponseModel
	{
		public BaseDeleteResponseModel(string? message = null) : base()
		{
			if (message is not null)
			{
				Message = message;
			}
		}

		public BaseDeleteResponseModel(Form form, string? message = null) : base(form)
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
