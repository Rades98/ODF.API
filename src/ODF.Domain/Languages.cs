using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace ODF.Domain
{
	public class Language
	{
		private readonly RegionInfo _regionInfo;

		public Language(int id, CultureInfo culture, string name)
		{
			Id = id;
			Culture = culture;
			_regionInfo = new(Culture.LCID);
			Name = name;
		}

		public int Id { get; }

		public CultureInfo Culture { get; }

		public string Name { get; }

		public string GetCountryCode() => _regionInfo.TwoLetterISORegionName.ToLower();
	}

	/// <summary>
	/// Languages enum
	/// </summary>
	public static class Languages
	{
		public static readonly Language Czech = new(0, new("cs-CZ"), "Čeština");
		public static readonly Language English = new(1, new("en-GB"), "English");
		public static readonly Language Deutsch = new(2, new("de-DE"), "Deutsch");

		public static IEnumerable<Language> GetAll()
		{
			var fields = typeof(Languages).GetFields(BindingFlags.Public |
			BindingFlags.Static |
			BindingFlags.DeclaredOnly);
			return fields.Select(f => f.GetValue(null)).Cast<Language>();
		}

		/// <summary>
		/// Try parse
		/// </summary>
		/// <param name="countryCode">country code</param>
		/// <param name="language">out language</param>
		/// <returns>true if found</returns>
		public static bool TryParse(string countryCode, out Language? language)
		{
			string code = countryCode;
			var lang = GetAll().FirstOrDefault(language => language.Culture.Name.ToLower().Contains(code));

			if (lang is not null)
			{
				language = lang;
				return true;
			}

			language = null;

			return false;
		}
	}
}
