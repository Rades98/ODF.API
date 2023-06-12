using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Mime;

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
			context.HttpContext.Response.ContentType = MediaTypeNames.Application.Json;
			await context.HttpContext.Response.WriteAsync(ResponseModel.ToString()!);
		}
	}
}
