using Newtonsoft.Json;
using ODF.API.ResponseModels.Common.Forms;

namespace ODF.API.ResponseModels.Base
{
	public class BaseCreateResponseModel : BaseResponseModel
	{
		public BaseCreateResponseModel(string? message = null, Form? form = null) : base(form)
		{
			if (message is not null)
			{
				Message = message;
			}

			if (form is not null)
			{
				if (form.Props.Any(prop => !string.IsNullOrEmpty(prop.ErrorMessage)))
				{
					Message = "Nevalidní vstup";
				}
			}
		}

		[JsonProperty("message", Required = Required.Always)]
		public string Message { get; set; } = "Záznam byl úspěšně vytvořen";
	}
}
