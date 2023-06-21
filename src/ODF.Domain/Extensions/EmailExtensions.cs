using System;
using System.Text.RegularExpressions;

namespace ODF.Domain.Extensions
{
	public static class EmailExtensions
	{
		private static Regex EmailRegex = new(@"^(?:[\w-]+(?:\.[\w-]+)*)@(?:(?:[\w-]+\.)*\w[\w-]{0,66})\.(?:[a-z]{2,6}(?:\.[a-z]{2})?)$", RegexOptions.Compiled, TimeSpan.FromSeconds(20));

		public static bool ValidateEmail(this string email)
			=> EmailRegex.IsMatch(email);
	}
}
