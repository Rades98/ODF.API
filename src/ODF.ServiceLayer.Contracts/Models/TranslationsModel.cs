using System.Collections.Generic;

namespace ODF.ServiceLayer.Contracts.Models
{
	public class TranslationsModel
	{
		public IEnumerable<TranslationModel> Translations { get; set; }

		public long Count { get; set; }
	}
}
