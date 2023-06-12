using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.ResponseCompression;
using ODF.API.Middleware;
using ODF.API.Registration.SettingModels;
using ODF.AppLayer.Settings;
using ODF.Data.Elastic.Settings;
using ODF.Data.Redis.Settings;
using System.IO.Compression;

namespace ODF.API.Registration
{
	internal static class ServicesRegistration
	{
		internal static IServiceCollection RegisterAppServices(this IServiceCollection services, IConfiguration conf, IWebHostEnvironment env)
		{
			services.ConfigureElasticsearch(conf)
					.ConfigureRedis(conf)
					.AddAppLayerServices();

			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
					.AddCookie(opts =>
					{
						opts.ExpireTimeSpan = TimeSpan.FromDays(1);
						opts.SlidingExpiration = true;
						opts.Cookie.HttpOnly = false;
						opts.Cookie.Name = "folklorova-auth_cookie";
						opts.Cookie.SameSite = SameSiteMode.None;

					});
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
				opts.ExcludedMimeTypes = _excludedMimeTypes;
			});

			services.AddTransient<IAuthorizationMiddlewareResultHandler, CustomAuthorizationResultMiddlewareHandler>();

			services.Configure<AntiScrappingSettings>(conf.GetSection(nameof(AntiScrappingSettings)));
			services.Configure<ApiSettings>(conf.GetSection(nameof(ApiSettings)));

			return services;
		}

		private static List<string> _excludedMimeTypes = new()
		{
			"application/javascript",
			"application/msword",
			"application/octet-stream",
			"application/ogg",
			"application/pdf",
			"application/rtf",
			"application/vnd.apple.installer+xml",
			"application/vnd.mozilla.xul+xml",
			"application/vnd.ms-excel",
			"application/vnd.ms-fontobject",
			"application/vnd.ms-powerpoint",
			"application/vnd.oasis.opendocument.presentation",
			"application/vnd.oasis.opendocument.spreadsheet",
			"application/vnd.oasis.opendocument.text",
			"application/vnd.openxmlformats-officedocument.presentationml.presentation",
			"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
			"application/vnd.openxmlformats-officedocument.wordprocessingml.document",
			"application/vnd.visio",
			"application/x-csh",
			"application/x-sh",
			"application/x-shockwave-flash",
			"application/xhtml+xml",
			"application/xml",
			"audio/3gpp",
			"audio/3gpp2",
			"audio/aac",
			"audio/midi",
			"audio/x-midi",
			"audio/mpeg",
			"audio/ogg",
			"audio/opus",
			"audio/wav",
			"audio/webm",
			"font/otf",
			"font/ttf",
			"font/woff",
			"font/woff2",
			"image/bmp",
			"image/gif",
			"image/jpeg",
			"image/png",
			"image/svg+xml",
			"image/tiff",
			"image/vnd.microsoft.icon",
			"image/webp",
			"text/calendar",
			"text/css",
			"text/csv",
			"text/html",
			"text/javascript",
			"text/plain",
			"text/xml",
			"video/3gpp",
			"video/3gpp2",
			"video/mp2t",
			"video/mpeg",
			"video/ogg",
			"video/webm",
			"video/x-msvideo",
		};
	}
}
