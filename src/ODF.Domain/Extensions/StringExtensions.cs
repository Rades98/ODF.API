using System;

namespace ODF.Domain.Extensions
{
	public static class StringExtensions
	{
		public static string ToCamelCase(this string input)
			=> string.Concat(input[0].ToString().ToLower(), input.AsSpan(1).ToString());
	}
}
