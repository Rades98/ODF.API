using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using ODF.API.Extensions;
using ODF.API.HealthChecks;
using ODF.API.Middleware;
using ODF.API.Registration;
using ODF.API.Registration.SpecificOptions;
using ODF.API.SignalR;
using ODF.API.SignalR.Hubs;
using ODF.API.SignalR.Registration;
using ODF.API.Swagger;
using ODF.Domain.Constants;
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
				.RegisterSignalR()
				.AddDistributedMemoryCache()
				.AddMemoryCache()
				.AddEndpointsApiExplorer()
				.AddCors()
				.AddHttpContextAccessor()
				.AddControllers(opts =>
				{
					opts.Conventions.Add(new RouteTokenTransformerConvention(new CamelCaseRouteTransformer()));
				})
				.AddNewtonsoftJson();

//metrics
builder.Services.AddOpenTelemetry()
	.WithMetrics(builder => builder
		.AddAspNetCoreInstrumentation()
		.AddHttpClientInstrumentation()
		.AddRuntimeInstrumentation()
		.AddConsoleExporter()
		.AddPrometheusExporter());

//Health checks
builder.Services.AddHealthChecks()
	.AddCheck<ElasticHealthCheck>("Elastic_DB")
	.AddCheck<RedisHealthCheck>("Redis_DB");

//logging
builder.Host.UseSerilog();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.SetupSwagger();
	app.UseCors(x => x
	.AllowAnyMethod()
	.AllowAnyHeader()
	.AllowCredentials()
	.WithOrigins("http://localhost:4200"));
}
else
{
	app.UseCors(x => x
	.AllowAnyMethod()
	.AllowAnyHeader()
	.AllowCredentials()
	.WithOrigins("https://folklorova.cz"));
}

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
		app.UseHttpsRedirection();
		appBuilder.UseResponseCompression();
	}
);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("", [Authorize][AllowAnonymous] () => Results.Redirect("/api/cz/navigation", true, true));

//TODO move to some other location like create chatting controller
app.MapPost("{countryCode}/sendMsgToAll", [Authorize(Roles = UserRoles.Admin)] async ([FromQuery] string countryCode, [FromQuery] string msg, IHubContext<ChatHub> hubContext, CancellationToken ct) =>
{
	await hubContext.Clients.All.SendAsync("ReceiveBroadcastMessage", msg, cancellationToken: ct);

	return Results.Ok();
});
app.MapPost("{countryCode}/sendDirect", [Authorize] async ([FromQuery] string countryCode, [FromQuery] string msg, [FromQuery] string userName, IHubContext<ChatHub> hubContext, HttpContext context, CancellationToken ct) =>
{
	await hubContext.Clients.User(userName).SendAsync("ReceiveDirectMessage", new ChatMessage(context.GetUserName(), userName, msg), cancellationToken: ct);
	return Results.Ok();
});

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<AuthCookieRevokeMiddleware>();
app.UseMiddleware<CountryCodeMiddleWare>();
app.UseMiddleware<RateLimitMiddleware>();
app.UseMiddleware<ResponseSelfMiddleware>();

app.SetupLogging();

app.UseHealthChecksPrometheusExporter(new("/health"), options => options.ResultStatusCodes[HealthStatus.Unhealthy] = (int)HttpStatusCode.OK);
app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.SetupSignalR(builder.Configuration);

app.Run();
