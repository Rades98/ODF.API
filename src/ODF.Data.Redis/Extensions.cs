namespace ODF.Data.Redis
{
	public static class Extensions
	{
		public static string GetRedisKey(this object obj, string name) => $"{obj.GetType().Name}_{name}";
	}
}
