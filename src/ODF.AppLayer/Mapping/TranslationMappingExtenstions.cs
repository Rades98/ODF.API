using System.Collections.Generic;
using System.Linq;
using ODF.AppLayer.Dtos;
using ODF.Domain.Entities;

namespace ODF.AppLayer.Mapping
{
	internal static class TranslationMappingExtenstions
	{
		internal static TranslationDto ToDto(this Translation translation)
			=> new()
			{
				Text = translation.Text,
				TranslationCode = translation.TranslationCode
			};

		internal static IEnumerable<TranslationDto> ToDtos(this IEnumerable<Translation> translations)
			=> translations.Select(ToDto);
	}
}
