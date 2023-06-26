using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ODF.AppLayer.Repos;
using ODF.Data.Redis.Settings;
using ODF.Domain.Constants;
using ODF.Domain.Entities;
using ODF.Domain.Utils;
using StackExchange.Redis;

namespace ODF.Data.Redis.Repos
{
	internal class UserRepo : IUserRepo
	{
		private readonly IDatabase _redisDb;
		private readonly IServer _redisServer;

		public UserRepo(IConnectionMultiplexer connectionMultiplexer, RedisSettings settings)
		{
			_ = connectionMultiplexer ?? throw new ArgumentNullException(nameof(connectionMultiplexer));
			_ = settings ?? throw new ArgumentNullException(nameof(settings));
			_redisDb = connectionMultiplexer.GetDatabase();
			_redisServer = connectionMultiplexer.GetServer(settings.Url);
		}

		public Task<bool> ChangePasswordAsync(string oldPwHash, string oldPwSalt, string newPwHash, string newPwSalt, CancellationToken cancellationToken) => throw new NotImplementedException();

		public Task<User?> GetUserAsync(string userName, CancellationToken cancellationToken)
			=> GetUsrAsync(userName, true);

		public async Task<IEnumerable<string>> GetUserNamesAsync(CancellationToken cancellationToken)
		{
			var names = new ConcurrentStack<string>();
			var keys = _redisServer.Keys(pattern: "*").ToList();
			var keyTasks = keys.Select(key => Task.Run(async () =>
			{
				var usr = await _redisDb.StringGetAsync(key);
				if (!string.IsNullOrEmpty(usr))
				{
					var user = JsonConvert.DeserializeObject<User>(usr)!;

					names.Push(user.UserName);
				}
			}));


			await Task.WhenAll(keyTasks);

			return names;
		}

		public async Task<string> RegisterAsync(string userName, string email, bool isAdmin, string passwordHash, CancellationToken cancellationToken)
		{
			string changeHash = Hasher.GetMailHashString($"{RegistrationHash.HashPrefix}{userName}");

			User usr = new()
			{
				UserName = userName,
				Email = email,
				IsAdmin = isAdmin,
				PasswordHash = passwordHash,
				ChangeHashValidTo = DateTime.UtcNow.AddDays(2),
				ChangeHash = changeHash
			};

			if (await _redisDb.StringSetAsync(usr.GetRedisKey(userName), JsonConvert.SerializeObject(usr)))
			{
				return changeHash;
			}

			return string.Empty;
		}

		public async Task<bool> ActivateRegistration(string userName, CancellationToken cancellationToken)
		{
			var usr = await GetUsrAsync(userName, false);
			if (usr is not null)
			{
				usr.IsActive = true;

				return await _redisDb.StringSetAsync(usr.GetRedisKey(userName), JsonConvert.SerializeObject(usr));
			}

			return false;
		}

		public async Task<IEnumerable<string>> GetUserEmailsAsync(CancellationToken cancellationToken)
		{
			var emails = new ConcurrentStack<string>();
			var keys = _redisServer.Keys(pattern: "*").ToList();
			var keyTasks = keys.Select(key => Task.Run(async () =>
			{
				var usr = await _redisDb.StringGetAsync(key);
				if (!string.IsNullOrEmpty(usr))
				{
					var user = JsonConvert.DeserializeObject<User>(usr)!;

					emails.Push(user.Email);
				}
			}));


			await Task.WhenAll(keyTasks);

			return emails;
		}

		public async Task<User?> GetUserByHashAsync(string hash, CancellationToken cancellationToken)
		{
			var keys = _redisServer.Keys(pattern: "*").ToList();
			User? resUser = null;

			var keyTasks = keys.Select(key => Task.Run(async () =>
			{
				var usr = await _redisDb.StringGetAsync(key);
				if (!string.IsNullOrEmpty(usr))
				{
					var user = JsonConvert.DeserializeObject<User>(usr)!;

					if (user.ChangeHash == hash)
					{
						resUser = user;
					}
				}
			}));

			await Task.WhenAll(keyTasks);

			return resUser ?? null;
		}

		private async Task<User?> GetUsrAsync(string userName, bool active = true)
		{
			var usr = await _redisDb.StringGetAsync(new User().GetRedisKey(userName));

			if (!string.IsNullOrEmpty(usr))
			{
				var user = JsonConvert.DeserializeObject<User>(usr);

				return (user?.IsActive == active) ? user : null;
			}

			return null;
		}
	}
}
