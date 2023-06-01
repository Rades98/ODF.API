namespace ODF.Data.Redis
{
	public static class Extensions
	{
		public static string GetRedisKey(this object obj, string name) => $"{nameof(obj)}_{name}";
	}
}
