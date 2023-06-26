using System;
using System.Text.RegularExpressions;

namespace ODF.Domain.Extensions
{
	public static class StringExtensions
	{
		private static Regex EmailRegex = new(@"^(?:[\w-]+(?:\.[\w-]+)*)@(?:(?:[\w-]+\.)*\w[\w-]{0,66})\.(?:[a-z]{2,6}(?:\.[a-z]{2})?)$", RegexOptions.Compiled, TimeSpan.FromSeconds(20));

		private static Regex PasswordRegex = new(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+|\/\\\[\]\{\};',.])[A-Za-z\d!@#$%^&*()_+|\/\\\[\]\{\};',.]{8,}$", RegexOptions.Compiled, TimeSpan.FromSeconds(20));

		private static readonly Regex CzIBANRegex = new(@"^(?:CZ|cz)(?:\d{22}|\d{2}[ ]{1}(?:\d{4}[ ]){4}\d{4})$", RegexOptions.Compiled, TimeSpan.FromSeconds(20));

		private readonly static Regex PostalCodeRegex = new(@"^(?:\d{3}\s\d{2})|(?:\d{5})$", RegexOptions.Compiled, TimeSpan.FromSeconds(20));

		public static bool ValidateEmail(this string email)
			=> EmailRegex.IsMatch(email);

		public static bool ValidatePassword(this string pw)
			=> PasswordRegex.IsMatch(pw);

		public static bool ValidateIban(this string iban)
			=> CzIBANRegex.IsMatch(iban);

		public static bool ValidatePostalCode(this string postalCode)
			=> PostalCodeRegex.IsMatch(postalCode);

		public static string ToCamelCase(this string input)
			=> string.Concat(input[0].ToString().ToLower(), input.AsSpan(1).ToString());
	}
}
