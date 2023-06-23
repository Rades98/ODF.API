using Microsoft.Extensions.FileProviders;

namespace ODF.API.Swagger
{
	public static class SwaggerRegistration
	{
		public static IApplicationBuilder SetupSwagger(this IApplicationBuilder app)
		{
			app.UseFileServer(new FileServerOptions()
			{
				FileProvider = new PhysicalFileProvider(Path.Combine(AppContext.BaseDirectory, @"Swagger")),
				RequestPath = new PathString("/swagger-scripts")
			});

			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				string jsPath = "/swagger-scripts/swagger-auth.js";
				c.InjectJavascript("https://code.jquery.com/jquery-3.6.0.min.js");
				c.InjectJavascript("https://cdn.jsdelivr.net/npm/swagger-ui-dist@3.52.0/swagger-ui-bundle.js");
				c.InjectJavascript(jsPath);
			});

			return app;
		}

		public static IServiceCollection SetupSwagger(this IServiceCollection services)
		{
			services.AddSwaggerGen();

			return services;
		}
	}
}
