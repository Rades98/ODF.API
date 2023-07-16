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
		private static Regex SelfReg = new(@"""href"":\s*(""[^""]+"")\s*,\s*""rel""\s*:\s*""_self""\s*,\s*""method""\s*:\s*""""", RegexOptions.Compiled, TimeSpan.FromSeconds(20));
		private static readonly JsonSerializerSettings _jsonSerializerSettings = new() { Error = (sender, args) => { args.ErrorContext.Handled = true; } };

		private readonly RequestDelegate _next;
		private readonly ApiSettings _apiSettings;

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
			if (!httpContext.IsApiRequest() || JsonConvert.DeserializeObject<ApiModel>(response, _jsonSerializerSettings) is null)
			{
				return response;
			}

			string resplacement = $"\"href\": \"{new Uri($"{_apiSettings.ApiUrl}{httpContext.Request.Path}")}\", \"rel\":\"self\", \"method\": \"{httpContext.Request.Method}\"";

			return SelfReg.Replace(response, resplacement);
		}
	}
}
