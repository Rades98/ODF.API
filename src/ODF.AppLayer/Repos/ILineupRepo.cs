using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ODF.Domain.Entities;

namespace ODF.AppLayer.Repos
{
	public interface ILineupRepo
	{
		Task<IEnumerable<LineupItem>> GetLineupAsync(CancellationToken cancellationToken);

		Task<bool> AddLineupItemAsync(LineupItem lineupItem, CancellationToken cancellationToken);
	}
}
