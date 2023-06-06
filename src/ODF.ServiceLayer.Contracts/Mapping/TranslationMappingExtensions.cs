using System.Collections.Generic;
using System.Linq;
using ODF.Data.Contracts.Entities;
using ODF.ServiceLayer.Contracts.Models;

namespace ODF.ServiceLayer.Contracts.Mapping
{
	public static class TranslationMappingExtensions
	{
		public static TranslationModel ToModel(this Translation translation)
			=> new()
			{
				Text = translation.Text,
				TranslationCode = translation.TranslationCode
			};

		public static IEnumerable<TranslationModel> ToModels(this IEnumerable<Translation> translations)
			=> translations.Select(ToModel);
	}
}
