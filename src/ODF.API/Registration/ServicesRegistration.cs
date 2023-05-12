using System;
using System.IO.Compression;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.ResponseCompression;
using ODF.API.Middleware;
using ODF.API.Registration.SettingModels;
using ODF.AppLayer.Settings;
using ODF.Data.Elastic.Settings;

namespace ODF.API.Registration
{
	internal static class ServicesRegistration
	{
		internal static IServiceCollection RegisterAppServices(this IServiceCollection services, IConfiguration conf, IWebHostEnvironment env)
		{
			services.ConfigureElasticsearch(conf)
					.AddAppLayerServices();

			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
					.AddCookie(options =>
					{
						options.ExpireTimeSpan = TimeSpan.FromDays(1);
						options.SlidingExpiration = true;
						options.Cookie.HttpOnly = false;
						options.Cookie.Name = "folklorova-auth_cookie";
						options.Cookie.SameSite = SameSiteMode.None;

					});
			services.AddDataProtection()
					.SetApplicationName($"folklor-ova-{env.EnvironmentName}")
					.PersistKeysToFileSystem(new DirectoryInfo($@"{env.ContentRootPath}\keys"));

			services.Configure<GzipCompressionProviderOptions>(opt =>
			{
				opt.Level = CompressionLevel.Fastest;
			});

			services.AddResponseCompression(options =>
			{
				options.EnableForHttps = true;
				options.Providers.Add<GzipCompressionProvider>();
			});

			services.AddTransient<IAuthorizationMiddlewareResultHandler, CustomAuthorizationResultMiddlewareHandler>();

			services.Configure<AntiScrappingSettings>(conf.GetSection(nameof(AntiScrappingSettings)));
			services.Configure<ApiSettings>(conf.GetSection(nameof(ApiSettings)));

			return services;
		}
	}
}
