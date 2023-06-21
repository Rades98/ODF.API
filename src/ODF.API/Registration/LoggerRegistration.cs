using System.Reflection;
using ODF.API.Registration.SettingModels;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

namespace ODF.API.Registration
{
	public static class LoggerRegistration
	{
		public static WebApplication SetupLogging(this WebApplication app)
		{
			var configuration = app.Configuration;
			var environment = app.Environment.EnvironmentName;

			Log.Logger = new LoggerConfiguration()
				.Enrich.FromLogContext()
				.Enrich.WithExceptionDetails()
				.WriteTo.Debug()
				.WriteTo.Console()
				.WriteTo.Elasticsearch(ConfigureElasticSink(configuration, environment))
				.Enrich.WithProperty("Environment", environment)
				.ReadFrom.Configuration(configuration)
				.CreateLogger();

			app.UseSerilogRequestLogging(options =>
			{
				options.MessageTemplate = "{RemoteIpAddress} {RequestScheme} {RequestHost} {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";

				options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
				{
					diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
					diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
					diagnosticContext.Set("RemoteIpAddress", httpContext.Connection.RemoteIpAddress);
				};
			});

			return app;
		}

		private static ElasticsearchSinkOptions ConfigureElasticSink(IConfiguration configuration, string environment)
		{
			var elasticConf = configuration.GetSection(nameof(ElasticsearchSettings)).Get<ElasticsearchSettings>();
			_ = elasticConf ?? throw new ArgumentException(nameof(elasticConf));

			return new ElasticsearchSinkOptions(new Uri(elasticConf.Nodes.First()))
			{
				ModifyConnectionSettings = x => x.BasicAuthentication("elastic", elasticConf.Password),
				AutoRegisterTemplate = true,
				IndexFormat = $"{(Assembly.GetExecutingAssembly().GetName().Name ?? "odf-api").ToLower().Replace(".", "-")}-{environment?.ToLower().Replace(".", "-")}"
			};
		}
	}
}
