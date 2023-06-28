using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ODF.API.Extensions;
using ODF.API.ResponseModels.Common;
using ODF.Domain.SettingModels;

namespace ODF.API.Middleware
{
	public class ResponseSelfMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ApiSettings _apiSettings;

		private static readonly JsonSerializerSettings _jsonSerializerSettings = new() { Error = (sender, args) => { args.ErrorContext.Handled = true; } };
		private static Regex SelfReg = new(@"""_self""\s*:\s*{[^}]*}}", RegexOptions.Compiled, TimeSpan.FromSeconds(20));

		public ResponseSelfMiddleware(RequestDelegate next, IOptions<ApiSettings> apiSettings)
		{
			_next = next;
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
				string responseBody = GetModifiedResponse(new StreamReader(memStream).ReadToEnd(), httpContext);

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

		private string GetModifiedResponse(string response, HttpContext httpContext)
		{
			if (!httpContext.IsApiRequest())
			{
				return response;
			}

			var responseBody = JsonConvert.DeserializeObject<ApiModel>(response, _jsonSerializerSettings);

			if (responseBody is null)
			{
				return response;
			}

			string method = httpContext.Request.Method;

			var link = httpContext.Request.Path;

			responseBody.Self.Curl.Href = new Uri($"{_apiSettings.ApiUrl}{link}");
			responseBody.Self.Curl.Method = method;
			responseBody.Self.Curl.Rel = "self";
			string stringResposne = JsonConvert.SerializeObject(responseBody);

			return SelfReg.Replace(response, stringResposne[1..^1]);
		}
	}
}
