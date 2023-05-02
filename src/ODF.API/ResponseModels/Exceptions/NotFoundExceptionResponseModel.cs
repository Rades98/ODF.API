using ODF.API.ResponseModels.Common;

namespace ODF.API.ResponseModels.Exceptions
{
	public class NotFoundExceptionResponseModel : ExceptionResponseModel
	{
		public NotFoundExceptionResponseModel(string title, string message, NamedAction? altAction = null) : base(title, message, altAction)
		{
		}
	}
}
