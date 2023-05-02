using ODF.API.ResponseModels.Exceptions;

namespace ODF.API.Responses
{
	public static class CustomApiResponses
	{
		public static IResult InternalServerError(ExceptionResponseModel response)
			=> new ApiResult(System.Net.HttpStatusCode.InternalServerError, response);

		public static IResult NotFound(NotFoundExceptionResponseModel response)
			=> new ApiResult(System.Net.HttpStatusCode.NotFound, response);
		
		public static IResult BadRequest(BadRequestExceptionResponseModel response)
			=> new ApiResult(System.Net.HttpStatusCode.BadRequest, response);

		public static IResult Unauthorized(UnauthorizedExceptionResponseModel response)
			=> new ApiResult(System.Net.HttpStatusCode.Unauthorized, response);
	}
}
