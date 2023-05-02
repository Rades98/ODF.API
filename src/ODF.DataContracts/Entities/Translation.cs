namespace ODF.Data.Contracts.Entities
{
	public class Translation
	{
		public string TranslationCode { get; set; }

		public string Text { get; set; } = string.Empty;

		public int LanguageId { get; set; }
	}
}
