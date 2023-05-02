using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace ODF.API.Extensions
{
	public static class DistributedCacheExtensions
	{
		public async static Task<T?> GetCachedValueAsyn<T>(this IDistributedCache cache, string key, CancellationToken cancellationToken) where T : class, new()
		{
			var result = await cache.GetAsync(key, cancellationToken);

			if (result is null)
			{
				return null;
			}

			return JsonConvert.DeserializeObject<T>(Encoding.Default.GetString(result))!;
		}

		public async static Task SetCachedValueAsync<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions? opts = null, CancellationToken cancellationToken = default)
		{
			if(opts == null)
			{
				opts = new()
				{
					AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(20),
				};
			}

			byte[] cacheVal;

			if (value == null)
			{
				cacheVal = null;
			}

			cacheVal =  Encoding.Default.GetBytes(JsonConvert.SerializeObject(value));

			await cache.SetAsync(key, cacheVal, opts, cancellationToken);
		}
	}
}
