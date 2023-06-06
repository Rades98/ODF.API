using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using ODF.API.Filters;
using ODF.API.Middleware;
using ODF.API.Registration;
using ODF.API.Registration.SpecificOptions;
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

builder.Host.UseSerilog();

var app = builder.Build();

ServiceLocator.Instance = app.Services;

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
	app.UseCors(x => x
	.AllowAnyMethod()
	.AllowAnyHeader()
	.SetIsOriginAllowed(origin => true)
	.AllowCredentials());
}
else
{
	app.UseCors(x => x
	.AllowAnyOrigin()
	.AllowAnyMethod()
	.AllowAnyHeader()
	.AllowCredentials()
	.WithOrigins("https://mycooldomain.lol"));
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

app.Run();
