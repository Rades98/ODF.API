namespace ODF.AppLayer.Services.Interfaces
{
	public interface IPasswordHasher
	{
		public string Hash(string password);

		public (bool Verified, bool NeedsUpgrade) Check(string hash, string password);
	}
}
