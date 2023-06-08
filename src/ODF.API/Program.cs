using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using ODF.API.Extensions;
using ODF.API.Filters;
using ODF.API.HealthChecks;
using ODF.API.Middleware;
using ODF.API.Registration;
using ODF.API.Registration.SpecificOptions;
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
				.AddSwaggerGen()
				.AddCors()
				.AddHttpContextAccessor()
				.AddControllers(opts =>
				{
					opts.Filters.Add<PropertyBindingActionFilterAttribute>();
					opts.Conventions.Add(new RouteTokenTransformerConvention(new CamelCaseRouteTransformer()));
				});

//metrics
builder.Services.AddOpenTelemetry()
	.WithMetrics(builder => builder
		.AddConsoleExporter()
		.AddAspNetCoreInstrumentation()
		.AddRuntimeInstrumentation()
		.AddPrometheusExporter());

//Health checks
builder.Services.AddHealthChecks()
	.AddCheck<ElasticHealthCheck>("Elastic_DB")
	.AddCheck<RedisHealthCheck>("Redis_DB");

//logging
builder.Host.UseSerilog();

var app = builder.Build();

app.UseOpenTelemetryPrometheusScrapingEndpoint();

ServiceLocator.Instance = app.Services;

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
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
		}
	);
}
else
{
	app.UseCors(x => x
	.AllowAnyOrigin()
	.AllowAnyMethod()
	.AllowAnyHeader()
	.AllowCredentials()
	.WithOrigins("https://folklorova.cz"));

	app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("", [Authorize][AllowAnonymous] () => Results.Redirect("/cz/navigation", true, true));

app.UseResponseCompression();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<CountryCodeMiddleWare>();
app.UseMiddleware<RateLimitMiddleware>();
app.UseMiddleware<LoggingMiddleware>();
app.UseMiddleware<ResponseSelfMiddleware>();

app.SetupLogging();

app.MapHealthChecks("/health", new HealthCheckOptions
{
	ResponseWriter = HealthCheckExtensions.WriteResponse
});

app.Run();
