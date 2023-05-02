using System.IO.Compression;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ODF.API.Middleware;
using ODF.API.Registration.SettingModels;
using ODF.AppLayer.Settings;
using ODF.Data.Elastic.Settings;

namespace ODF.API.Registration
{
	internal static class ServicesRegistration
	{
		internal static IServiceCollection RegisterAppServices(this IServiceCollection services, IConfiguration conf)
		{
			services.ConfigureElasticsearch(conf)
					.AddAppLayerServices()
					.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
					.AddJwtBearer(o =>
					{
						o.TokenValidationParameters = new TokenValidationParameters
						{
							IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(conf["Jwt:Key"]!)),
							ValidateIssuer = false,
							ValidateAudience = false,
							ValidateIssuerSigningKey = true
						};
					});

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
