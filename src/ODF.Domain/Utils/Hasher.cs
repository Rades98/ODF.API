using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ODF.Domain.Utils
{
	public static class Hasher
	{
		private const int SaltSize = 16; // 128 bit 
		private const int KeySize = 32; // 256 bit
		private const int Iterations = 10000;

		public static string Hash(string password)
		{
			using var algorithm = new Rfc2898DeriveBytes(
			  password,
			  SaltSize,
			  Iterations,
			  HashAlgorithmName.SHA512);
			string key = Convert.ToBase64String(algorithm.GetBytes(KeySize));
			string salt = Convert.ToBase64String(algorithm.Salt);

			return $"{Iterations}.{salt}.{key}";
		}

		public static (bool Verified, bool NeedsUpgrade) Check(string hash, string password)
		{
			string[] parts = hash.Split('.', 3);

			if (parts.Length != 3)
			{
				throw new FormatException("Unexpected hash format. " +
				  "Should be formatted as `{iterations}.{salt}.{hash}`");
			}

			int iterations = Convert.ToInt32(parts[0]);
			byte[] salt = Convert.FromBase64String(parts[1]);
			byte[] key = Convert.FromBase64String(parts[2]);

			bool needsUpgrade = iterations != Iterations;

			using var algorithm = new Rfc2898DeriveBytes(
			  password,
			  salt,
			  iterations,
			  HashAlgorithmName.SHA512);
			byte[] keyToCheck = algorithm.GetBytes(KeySize);

			bool verified = keyToCheck.SequenceEqual(key);

			return (verified, needsUpgrade);
		}

		public static string GetMailHashString(string inputString)
		{
			StringBuilder sb = new StringBuilder();
			foreach (byte b in GetMailHash(inputString))
				sb.Append(b.ToString("X2"));

			return sb.ToString();
		}

		private static byte[] GetMailHash(string inputString)
		{
			using HashAlgorithm algorithm = SHA256.Create();
			return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
		}
	}
}
