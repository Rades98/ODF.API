namespace ODF.Data.Redis.Settings
{
	internal class RedisSettings
	{
		public string Url { get; set; } = string.Empty;

		public string Password { get; set; } = string.Empty;

		public string ConnectionString => $"{Url},password={Password}";
	}
}
