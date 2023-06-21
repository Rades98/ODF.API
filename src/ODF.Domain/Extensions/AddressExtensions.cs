using System;
using System.Text.RegularExpressions;

namespace ODF.Domain.Extensions
{
	public static class AddressExtensions
	{
		private readonly static Regex PostalCodeRegex = new(@"^(?:\d{3}\s\d{2})|(?:\d{5})$", RegexOptions.Compiled, TimeSpan.FromSeconds(20));

		public static bool ValidatePostalCode(this string postalCode)
			=> PostalCodeRegex.IsMatch(postalCode);
	}
}
