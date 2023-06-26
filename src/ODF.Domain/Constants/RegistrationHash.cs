namespace ODF.Domain.Constants
{
	public static class RegistrationHash
	{
		public static string HashPrefix => _hashPrefix;
		private readonly static string _hashPrefix = "__reg__";
	}
}
