using System.Collections.Generic;
using ODF.AppLayer.Mediator;

namespace ODF.AppLayer.CQRS.User.Queries
{
	public class GetAllUserNamesQuery : IQuery<IEnumerable<string>>
	{
	}
}
