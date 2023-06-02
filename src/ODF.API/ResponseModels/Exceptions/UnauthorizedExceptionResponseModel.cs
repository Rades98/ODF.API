using ODF.API.ResponseModels.Common;

namespace ODF.API.ResponseModels.Exceptions
{
	public class UnauthorizedExceptionResponseModel : ExceptionResponseModel
	{
		public UnauthorizedExceptionResponseModel(string title, string message, NamedAction? self = null, NamedAction? altAction = null) : base(title, message, self, altAction)
		{

		}
	}
}
