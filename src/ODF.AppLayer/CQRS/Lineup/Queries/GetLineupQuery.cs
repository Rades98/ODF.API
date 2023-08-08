using System.Collections.Generic;
using ODF.AppLayer.Dtos;
using ODF.AppLayer.Mediator;

namespace ODF.AppLayer.CQRS.Lineup.Queries
{
	public sealed record GetLineupQuery(string CountryCode) : IQuery<IEnumerable<LineupItemDto>>;
}
