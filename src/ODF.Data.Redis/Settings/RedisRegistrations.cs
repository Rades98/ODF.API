﻿using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ODF.Data.Contracts.Entities;
using ODF.Data.Contracts.Interfaces;
using ODF.Data.Redis.Repos;
using StackExchange.Redis;

namespace ODF.Data.Redis.Settings
{
	public static class RedisRegistrations
	{
		public static IServiceCollection ConfigureRedis(this IServiceCollection services, IConfiguration configuration)
		{
			var redisSettings = configuration.GetSection(nameof(RedisSettings)).Get<RedisSettings>()
				?? throw new ArgumentNullException(nameof(RedisSettings));

			services.AddSingleton<IConnectionMultiplexer>(opt => ConnectionMultiplexer
				.Connect(redisSettings.Url)
				.Seed());

			services.AddTransient<IUserRepo, UserRepo>();

			return services;
		}

		private static ConnectionMultiplexer Seed(this ConnectionMultiplexer connectionMultiplexer)
		{
			var db = connectionMultiplexer.GetDatabase();
			string userName = "admin";
			var existing = db.StringGet(new User().GetRedisKey(userName));

			if (string.IsNullOrEmpty(existing))
			{
				var usr = new User()
				{
					UserName = userName,
					Email = "admin@folklorova.cz",
					Id = Guid.Parse("5697F910-A490-47E2-B90B-C42C662178CA"),
					IsAdmin = true,
					PasswordHash = "",
					PasswordSalt = "",
					IsActive = true,
				};

				string usrSerialized = JsonConvert.SerializeObject(usr);

				db.StringSetAsync(usr.GetRedisKey(usr.UserName), usrSerialized);
			}

			return connectionMultiplexer;
		}
	}
}