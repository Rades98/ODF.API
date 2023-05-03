using ODF.API.Middleware;
using ODF.API.Registration;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
			.SetBasePath(AppContext.BaseDirectory)
			.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
			.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
			.AddEnvironmentVariables()
			.Build();

// Add services to the container.
builder.Services.RegisterAppServices(builder.Configuration)
				.AddDistributedMemoryCache()
				.AddMemoryCache()
				.AddEndpointsApiExplorer()
				.AddSwaggerGen()
				.AddControllers();

builder.Host.UseSerilog();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapApiEndpoints();

app.UseMiddleware<CountryCodeMiddleWare>();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RateLimitMiddleware>();

app.UseCors(x => x
	.AllowAnyOrigin()
	.AllowAnyMethod()
	.AllowAnyHeader());

app.UseResponseCompression();

app.SetupLogging();

app.Run();
