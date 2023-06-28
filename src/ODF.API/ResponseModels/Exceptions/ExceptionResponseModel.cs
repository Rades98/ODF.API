using Newtonsoft.Json;
using ODF.API.ResponseModels.Base;
using ODF.API.ResponseModels.Common;

namespace ODF.API.ResponseModels.Exceptions
{
	public class ExceptionResponseModel : BaseResponseModel
	{
		public ExceptionResponseModel(string title, string? message = null, NamedAction? altAction = null)
		{
			Title = title;
			Message = message;
			AlternativeAction = altAction;
		}

		[JsonProperty("title", Required = Required.Always)]
		public string Title { get; }

		[JsonProperty("message", Required = Required.AllowNull)]
		public string? Message { get; }

		[JsonProperty("alternativeAction", Required = Required.AllowNull)]
		public NamedAction? AlternativeAction { get; }

		public override string ToString() => JsonConvert.SerializeObject(this);
	}
}
