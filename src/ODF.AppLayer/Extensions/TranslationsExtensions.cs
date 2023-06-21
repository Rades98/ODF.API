using System.Collections.Generic;
using System.Linq;
using ODF.AppLayer.Dtos;
using ODF.AppLayer.Exceptions;

namespace ODF.AppLayer.Extensions
{
	public static class TranslationsExtensions
	{
		public static string Get(this IReadOnlyList<TranslationDto> translations, string translationCode)
		{
			string translation = translations.FirstOrDefault(tr => tr.TranslationCode == translationCode)?.Text;

			if (translation is not null)
			{
				return translation;
			}

			throw new MissingTranslationException($"there is no translation for code: {translationCode}");
		}
	}
}
