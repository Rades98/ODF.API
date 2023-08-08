using ODF.AppLayer.Dtos;
using ODF.AppLayer.Mediator;

namespace ODF.AppLayer.CQRS.Translations.Queries
{
	public sealed record GetTranslationsQuery(string CountryCode, int Size, int Offset) : IQuery<TranslationsDto>;
}
