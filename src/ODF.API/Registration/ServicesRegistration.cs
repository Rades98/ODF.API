using System.IO.Compression;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.ResponseCompression;
using ODF.API.Cookies;
using ODF.API.Middleware;
using ODF.API.Registration.SettingModels;
using ODF.API.Swagger;
using ODF.AppLayer.Settings;
using ODF.Data.Elastic.Settings;
using ODF.Data.Redis.Settings;
using ODF.Domain.Constants;
using ODF.Domain.SettingModels;

namespace ODF.API.Registration
{
	internal static class ServicesRegistration
	{
		internal static IServiceCollection RegisterAppServices(this IServiceCollection services, IConfiguration conf, IWebHostEnvironment env)
		{
			services.ConfigureElasticsearch(conf)
					.ConfigureRedis(conf)
					.AddAppLayerServices(conf);

			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
					.AddCookie(CookieProps.CookieAuthenticationOpts);

			services.AddDataProtection()
					.SetApplicationName($"folklor-ova-{env.EnvironmentName}")
					.PersistKeysToFileSystem(new DirectoryInfo($@"{env.ContentRootPath}\keys"));

			services.Configure<GzipCompressionProviderOptions>(opts =>
			{
				opts.Level = CompressionLevel.Fastest;
			});

			services.AddResponseCompression(opts =>
			{
				opts.EnableForHttps = true;
				opts.Providers.Add<GzipCompressionProvider>();
				opts.ExcludedMimeTypes = MimeTypes.ExcludedMimeTypes;
			});

			services.AddTransient<IAuthorizationMiddlewareResultHandler, CustomAuthorizationResultMiddlewareHandler>();

			services.Configure<AntiScrappingSettings>(conf.GetSection(nameof(AntiScrappingSettings)));
			services.Configure<ApiSettings>(conf.GetSection(nameof(ApiSettings)));

			services.SetupSwagger();

			return services;
		}
	}
}
