using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ODF.API.Registration.SettingModels;
using ODF.API.ResponseModels.Base;

namespace ODF.API.Middleware
{
	public class ResponseSelfMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly IActionDescriptorCollectionProvider _adcp;
		private readonly ApiSettings _apiSettings;
		private static Regex SelfReg = new(@"(?:(!:[""]_self).)*(?:[""]_self).*[}}\]}]$", RegexOptions.Compiled);

		public ResponseSelfMiddleware(RequestDelegate next, IActionDescriptorCollectionProvider adcp, IOptions<ApiSettings> apiSettings)
		{
			_next = next;
			_adcp = adcp;
			_apiSettings = apiSettings.Value;
		}

		public async Task Invoke(HttpContext httpContext)
		{
			var originBody = httpContext.Response.Body;
			try
			{
				var memStream = new MemoryStream();
				httpContext.Response.Body = memStream;

				await _next(httpContext).ConfigureAwait(false);

				memStream.Position = 0;
				var responseBody = GetModifiedResponse(new StreamReader(memStream).ReadToEnd(), httpContext);

				var memoryStreamModified = new MemoryStream();
				var sw = new StreamWriter(memoryStreamModified);
				sw.Write(responseBody);
				sw.Flush();
				memoryStreamModified.Position = 0;

				await memoryStreamModified.CopyToAsync(originBody).ConfigureAwait(false);
			}
			finally
			{
				httpContext.Response.Body = originBody;
			}
		}

		//This stuff with regex and shit is some sort of hack.. would be nice if it was refactored once - but not today :D
		private string GetModifiedResponse(string response, HttpContext httpContext)
		{
			var responseBody = JsonConvert.DeserializeObject<BaseResponseModel>(response);

			if (responseBody is null)
			{
				return response;
			}

			var method = httpContext.Request.Method;

			var link = httpContext.Request.Path;

			responseBody._self.Curl.Href = new Uri($"{_apiSettings.ApiUrl}{link}");
			responseBody._self.Curl.Method = method;
			responseBody._self.Curl.Rel = "self";
			responseBody._self.ActionName = "_self";

			return SelfReg.Replace(response, JsonConvert.SerializeObject(responseBody));
		}
	}
}
