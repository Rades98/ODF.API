using ODF.API.ResponseModels.Common;

namespace ODF.API.ResponseModels.Exceptions
{
	public class BadRequestExceptionResponseModel : ExceptionResponseModel
	{
		public BadRequestExceptionResponseModel(string title, string message, NamedAction? altAction = null) : base(title, message, altAction)
		{
		}
	}
}
