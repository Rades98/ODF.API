using Newtonsoft.Json;
using ODF.API.ResponseModels.Common.Forms;

namespace ODF.API.ResponseModels.Base
{
	public class BaseCreateResponseModel : BaseResponseModel
	{
		public BaseCreateResponseModel(Form form) : base(form)
		{
			if (form is not null && form.Props.Any(prop => !string.IsNullOrEmpty(prop.ErrorMessage)))
			{
				Message = "Nevalidní vstup";
			}
		}

		public BaseCreateResponseModel(Form form, string? message = null) : base(form)
		{
			Message = message ?? "Nevalidní vstup"; ;
		}

		public BaseCreateResponseModel(string? message = null) : base()
		{
			if (message is not null)
			{
				Message = message;
			}
		}

		[JsonProperty("message", Required = Required.Always)]
		public string Message { get; set; } = "Záznam byl úspěšně vytvořen";
	}
}
