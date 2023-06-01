using System;
using System.Linq;
using System.Security.Cryptography;

namespace ODF.AppLayer.PWHashing
{
	internal static class PasswordUtils
	{
		public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
		{
			using var hmac = new HMACSHA512();
			passwordSalt = hmac.Key;
			passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
		}

		public static bool VerifyPasswordHash(string password, byte[] pwHash, byte[] pwSalt)
		{
			using var hmac = new HMACSHA512(pwSalt);
			byte[] computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
			return computedHash.SequenceEqual(pwHash);
		}
	}
}
