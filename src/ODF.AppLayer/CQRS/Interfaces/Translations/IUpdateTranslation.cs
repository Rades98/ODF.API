namespace ODF.AppLayer.CQRS.Interfaces.Translations
{
	public interface IUpdateTranslation
	{
		string CountryCode { get; }

		string TranslationCode { get; }

		string Text { get; }
	}
}
