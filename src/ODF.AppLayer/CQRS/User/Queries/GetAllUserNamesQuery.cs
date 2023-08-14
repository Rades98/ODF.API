using System.Collections.Generic;
using ODF.AppLayer.Mediator;

namespace ODF.AppLayer.CQRS.User.Queries
{
	public sealed record GetAllUserNamesQuery() : IQuery<IEnumerable<string>>;
}
