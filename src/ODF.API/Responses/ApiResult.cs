using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ODF.API.Responses
{
	[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
	public class ApiResult : IActionResult
	{
		public ApiResult(HttpStatusCode statusCode, object responseModel)
		{
			StatusCode = statusCode;
			ResponseModel = responseModel;
		}

		public HttpStatusCode StatusCode { get; }

		public object ResponseModel { get; }

		public async Task ExecuteResultAsync(ActionContext context)
		{
			context.HttpContext.Response.StatusCode = (int)StatusCode;
			context.HttpContext.Response.ContentType = "application/json";
			await context.HttpContext.Response.WriteAsync(ResponseModel.ToString()!);
		}
	}
}
