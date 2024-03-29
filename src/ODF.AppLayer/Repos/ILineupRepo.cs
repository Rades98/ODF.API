﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ODF.Domain.Entities;

namespace ODF.AppLayer.Repos
{
	public interface ILineupRepo
	{
		Task<IEnumerable<LineupItem>> GetLineupAsync(CancellationToken cancellationToken);

		Task<IEnumerable<LineupItem>> GetLineupAsync(string userName, CancellationToken cancellationToken);

		Task<bool> AddLineupItemAsync(LineupItem lineupItem, CancellationToken cancellationToken);

		Task<bool> RemoveLineupItemAsync(Guid id, CancellationToken cancellationToken);

		Task<bool> UpdateLineupItemAsync(LineupItem lineupItem, CancellationToken cancellationToken);

		Task<LineupItem> GetAsync(Guid id, CancellationToken cancellationToken);
	}
}
