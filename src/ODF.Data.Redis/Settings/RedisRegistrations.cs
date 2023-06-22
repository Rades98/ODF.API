using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ODF.AppLayer.Repos;
using ODF.Data.Redis.Repos;
using ODF.Domain.Entities;
using ODF.Domain.SettingModels;
using ODF.Domain.Utils;
using StackExchange.Redis;

namespace ODF.Data.Redis.Settings
{
	public static class RedisRegistrations
	{
		public static IServiceCollection ConfigureRedis(this IServiceCollection services, IConfiguration configuration)
		{
			var redisSettings = configuration.GetSection(nameof(RedisSettings)).Get<RedisSettings>()
				?? throw new ArgumentException(nameof(RedisSettings));

			var apiSettings = configuration.GetSection(nameof(ApiSettings)).Get<ApiSettings>()
				?? throw new ArgumentException(nameof(ApiSettings));

			services.AddSingleton<IConnectionMultiplexer>(opt => ConnectionMultiplexer
				.Connect(redisSettings.ConnectionString)
				.Seed(apiSettings.AdminPw));

			services.AddTransient<IUserRepo, UserRepo>();

			return services;
		}

		private static ConnectionMultiplexer Seed(this ConnectionMultiplexer connectionMultiplexer, string pw)
		{
			var db = connectionMultiplexer.GetDatabase();
			string userName = "admin";

			if (string.IsNullOrEmpty(db.StringGet(new User().GetRedisKey(userName))))
			{
				var usr = new User()
				{
					UserName = userName,
					Email = "admin@folklorova.cz",
					Id = Guid.Parse("5697F910-A490-47E2-B90B-C42C662178CA"),
					IsAdmin = true,
					PasswordHash = PasswordHasher.Hash(pw),
					IsActive = true,
				};

				string usrSerialized = JsonConvert.SerializeObject(usr);

				db.StringSetAsync(usr.GetRedisKey(usr.UserName), usrSerialized);
			}

			return connectionMultiplexer;
		}
	}
}
