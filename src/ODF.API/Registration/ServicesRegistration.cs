using System.IO.Compression;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
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
					.AddAuthentication(options =>
					{
						options.DefaultScheme = "JWT_OR_COOKIE";
						options.DefaultChallengeScheme = "JWT_OR_COOKIE";
					})
					.AddCookie(options =>
					{
						options.ExpireTimeSpan = TimeSpan.FromDays(1);
					})
					.AddJwtBearer(options =>
					{
						options.TokenValidationParameters = new TokenValidationParameters
						{
							ValidateIssuer = false,
							ValidateAudience = false,
							ValidateIssuerSigningKey = true,
							IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(conf["Jwt:Key"]!))
						};
					})
					.AddPolicyScheme("JWT_OR_COOKIE", "JWT_OR_COOKIE", options =>
					{
						options.ForwardDefaultSelector = context =>
						{
							string authorization = context.Request.Headers[HeaderNames.Authorization]!;
							if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer "))
								return JwtBearerDefaults.AuthenticationScheme;

							return CookieAuthenticationDefaults.AuthenticationScheme;
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
