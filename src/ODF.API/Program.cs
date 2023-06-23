using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using ODF.API.HealthChecks;
using ODF.API.Middleware;
using ODF.API.Registration;
using ODF.API.Registration.SpecificOptions;
using ODF.API.Swagger;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
			.SetBasePath(AppContext.BaseDirectory)
			.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
			.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
			.AddEnvironmentVariables()
			.Build();

builder.Services.RegisterAppServices(builder.Configuration, builder.Environment)
				.AddDistributedMemoryCache()
				.AddMemoryCache()
				.AddEndpointsApiExplorer()
				.AddCors()
				.AddHttpContextAccessor()
				.AddControllers(opts =>
				{
					opts.Conventions.Add(new RouteTokenTransformerConvention(new CamelCaseRouteTransformer()));
				});

//metrics
builder.Services.AddOpenTelemetry()
	.WithMetrics(builder => builder
		.AddAspNetCoreInstrumentation()
		.AddHttpClientInstrumentation()
		.AddRuntimeInstrumentation()
		.AddPrometheusExporter());

//Health checks
builder.Services.AddHealthChecks()
	.AddCheck<ElasticHealthCheck>("Elastic_DB")
	.AddCheck<RedisHealthCheck>("Redis_DB");

//logging
builder.Host.UseSerilog();

var app = builder.Build();

ServiceLocator.Instance = app.Services;

if (app.Environment.IsDevelopment())
{
	app.SetupSwagger();
	app.UseCors(x => x
	.AllowAnyMethod()
	.AllowAnyHeader()
	.SetIsOriginAllowed(origin => true)
	.AllowCredentials());

	app.UseWhen(
		delegate (HttpContext httpContext)
		{
			return !(httpContext.Request.Path.ToString().Contains("/metrics") ||
					httpContext.Request.Path.ToString().Contains("/health") ||
					httpContext.Request.Path == new PathString("/"));
		}
		,
		delegate (IApplicationBuilder appBuilder)
		{
			appBuilder.UseHttpsRedirection();
			appBuilder.UseResponseCompression();
		}
	);
}
else
{
	app.UseCors(x => x
	.AllowAnyMethod()
	.AllowAnyHeader()
	.AllowCredentials()
	.WithOrigins("https://folklorova.cz"));

	app.UseHttpsRedirection();
	app.UseWhen(
		delegate (HttpContext httpContext)
		{
			return !(httpContext.Request.Path.ToString().Contains("/metrics") ||
					httpContext.Request.Path.ToString().Contains("/health") ||
					httpContext.Request.Path == new PathString("/"));
		}
		,
		delegate (IApplicationBuilder appBuilder)
		{
			appBuilder.UseResponseCompression();
		}
	);
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("", [Authorize][AllowAnonymous] () => Results.Redirect("/cz/navigation", true, true));

app.UseMiddleware<ExceptionHandlingMiddleware>();
//app.UseMiddleware<AuthCookieRevokeMiddleware>();
app.UseMiddleware<CountryCodeMiddleWare>();
app.UseMiddleware<RateLimitMiddleware>();
app.UseMiddleware<ResponseSelfMiddleware>();

app.SetupLogging();

app.UseHealthChecksPrometheusExporter(new("/health"), options => options.ResultStatusCodes[HealthStatus.Unhealthy] = (int)HttpStatusCode.OK);
app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.Run();
