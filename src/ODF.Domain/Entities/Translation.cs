namespace ODF.Domain.Entities
{
	public class Translation
	{
		public string TranslationCode { get; set; }

		public string Text { get; set; } = string.Empty;

		public int LanguageId { get; set; }

		public bool IsSystem { get; set; } = false;
	}
}
