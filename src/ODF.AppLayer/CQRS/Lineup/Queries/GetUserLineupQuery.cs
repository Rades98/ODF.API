using System.Collections.Generic;
using ODF.AppLayer.Dtos;
using ODF.AppLayer.Mediator;

namespace ODF.AppLayer.CQRS.Lineup.Queries
{
	public sealed class GetUserLineupQuery : IQuery<IEnumerable<LineupItemDto>>
	{
		public GetUserLineupQuery(string userName, string countryCode)
		{
			UserName = userName;
			CountryCode = countryCode;
		}

		public string CountryCode { get; }

		public string UserName { get; }
	}
}
