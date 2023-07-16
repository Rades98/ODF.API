namespace ODF.AppLayer.CQRS.Interfaces.Translations
{
	public interface IUpdateTranslation
	{
		string TranslationCode { get; }

		string Text { get; }

		string CountryCode { get; }
	}
}
