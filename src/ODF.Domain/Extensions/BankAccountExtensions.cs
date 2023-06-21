using System;
using System.Text.RegularExpressions;

namespace ODF.Domain.Extensions
{
	public static class BankAccountExtensions
	{
		private static readonly Regex CzIBANRegex = new(@"^(?:CZ|cz)(?:\d{22}|\d{2}[ ]{1}(?:\d{4}[ ]){4}\d{4})$", RegexOptions.Compiled, TimeSpan.FromSeconds(20));

		public static bool ValidateIban(this string iban)
			=> CzIBANRegex.IsMatch(iban);
	}
}
