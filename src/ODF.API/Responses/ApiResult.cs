using System.Net;
using Newtonsoft.Json;

namespace ODF.API.Responses
{
	[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
	public class ApiResult : IResult
	{
		public ApiResult(HttpStatusCode statusCode, object responseModel)
		{
			StatusCode = statusCode;
			ResponseModel = responseModel;
		}

		public HttpStatusCode StatusCode { get; }

		public object ResponseModel { get; }

		public async Task ExecuteAsync(HttpContext httpContext)
		{
			httpContext.Response.StatusCode = (int)StatusCode;
			httpContext.Response.ContentType = "application/json";
			await httpContext.Response.WriteAsync(ResponseModel.ToString()!);
		}
	}
}
