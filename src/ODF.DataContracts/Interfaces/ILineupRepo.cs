using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ODF.Data.Contracts.Entities;

namespace ODF.Data.Contracts.Interfaces
{
	public interface ILineupRepo
	{
		Task<IEnumerable<LineupItem>> GetLineupAsync(CancellationToken cancellationToken);

		Task<bool> AddLineupItemAsync(LineupItem lineupItem, CancellationToken cancellationToken);
	}
}
