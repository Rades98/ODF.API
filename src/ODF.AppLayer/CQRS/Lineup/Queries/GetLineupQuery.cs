using System.Collections.Generic;
using ODF.AppLayer.Dtos;
using ODF.AppLayer.Mediator;

namespace ODF.AppLayer.CQRS.Lineup.Queries
{
	public sealed class GetLineupQuery : IQuery<IEnumerable<LineupItemDto>>
	{
		public GetLineupQuery(string countryCode)
		{
			CountryCode = countryCode;
		}

		public string CountryCode { get; }
	}
}
