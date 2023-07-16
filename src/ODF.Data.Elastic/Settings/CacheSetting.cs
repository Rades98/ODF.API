using System;
using Microsoft.Extensions.Caching.Memory;

namespace ODF.Data.Elastic.Settings
{
	internal static class CacheSetting
	{
		public static MemoryCacheEntryOptions MemoryCacheEntryOptions => _memoryCacheEntryOptions;

		private static MemoryCacheEntryOptions _memoryCacheEntryOptions = new()
		{
			AbsoluteExpiration = DateTime.Now.AddMinutes(30)
		};
	}
}
