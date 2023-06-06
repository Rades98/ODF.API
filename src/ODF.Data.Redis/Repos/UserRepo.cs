using System;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ODF.AppLayer.Repos;
using ODF.Domain.Entities;
using StackExchange.Redis;

namespace ODF.Data.Redis.Repos
{
	internal class UserRepo : IUserRepo
	{
		private readonly IDatabase _redisDb;

		public UserRepo(IConnectionMultiplexer connectionMultiplexer)
		{
			_ = connectionMultiplexer ?? throw new ArgumentNullException(nameof(connectionMultiplexer));
			_redisDb = connectionMultiplexer.GetDatabase();
		}

		public Task<bool> ChangePasswordAsync(string oldPwHash, string oldPwSalt, string newPwHash, string newPwSalt, CancellationToken cancellationToken) => throw new NotImplementedException();

		public async Task<User?> GetUserAsync(string userName, CancellationToken cancellationToken)
		{
			var usr = await _redisDb.StringGetAsync(new User().GetRedisKey(userName));

			if (!string.IsNullOrEmpty(usr))
			{
				return JsonConvert.DeserializeObject<User>(usr);
			}

			return null;
		}

		public async Task<bool> RegisterAsync(string userName, string email, bool isAdmin, string passwordHash, string passwordSalt)
		{
			var usr = new User()
			{
				UserName = userName,
				Email = email,
				IsAdmin = isAdmin,
				PasswordSalt = passwordSalt,
				PasswordHash = passwordHash
			};

			return await _redisDb.StringSetAsync(usr.GetRedisKey(userName), JsonConvert.SerializeObject(usr));
		}

	}
}
