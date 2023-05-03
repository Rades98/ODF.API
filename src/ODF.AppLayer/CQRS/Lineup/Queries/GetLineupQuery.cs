using System.Collections.Generic;
using MediatR;
using ODF.AppLayer.Dtos;

namespace ODF.AppLayer.CQRS.Lineup.Queries
{
	public class GetLineupQuery : IRequest<IEnumerable<LineupItemDto>>
	{
		public GetLineupQuery(string countryCode)
		{
			CountryCode = countryCode;
		}

		public string CountryCode { get; }
	}
}
