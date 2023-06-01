using Microsoft.AspNetCore.Mvc;
using ODF.API.ResponseModels.Exceptions;

namespace ODF.API.Responses
{
	public static class CustomApiResponses
	{
		public static IActionResult InternalServerError(ExceptionResponseModel response)
			=> new ApiResult(System.Net.HttpStatusCode.InternalServerError, response);

		public static IActionResult NotFound(NotFoundExceptionResponseModel response)
			=> new ApiResult(System.Net.HttpStatusCode.NotFound, response);

		public static IActionResult BadRequest(BadRequestExceptionResponseModel response)
			=> new ApiResult(System.Net.HttpStatusCode.BadRequest, response);

		public static IActionResult Unauthorized(UnauthorizedExceptionResponseModel response)
			=> new ApiResult(System.Net.HttpStatusCode.Unauthorized, response);
	}
}
