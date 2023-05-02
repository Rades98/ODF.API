using System.Collections.Generic;

namespace ODF.AppLayer.Dtos
{
	public class TranslationsDto
	{
		public IEnumerable<TranslationDto> Translations { get; set; }

		public long Count { get; set; }
	}
}
