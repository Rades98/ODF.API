﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ODF.AppLayer.CQRS.Lineup.Queries;
using ODF.AppLayer.Dtos;
using ODF.AppLayer.Mediator;
using ODF.AppLayer.Repos;
using ODF.Domain;

namespace ODF.AppLayer.CQRS.Lineup.QueryHandlers
{
	internal class GetLineupQueryHandler : IQueryHandler<GetLineupQuery, IEnumerable<LineupItemDto>>
	{
		private readonly ITranslationRepo _translationRepo;
		private readonly ILineupRepo _lineupRepo;

		public GetLineupQueryHandler(ITranslationRepo translationRepo, ILineupRepo lineupRepo)
		{
			_lineupRepo = lineupRepo ?? throw new ArgumentNullException(nameof(lineupRepo));
			_translationRepo = translationRepo ?? throw new ArgumentNullException(nameof(translationRepo));
		}

		public async Task<IEnumerable<LineupItemDto>> Handle(GetLineupQuery request, CancellationToken cancellationToken)
		{
			var lineupItems = await _lineupRepo.GetLineupAsync(cancellationToken);

			var result = new List<LineupItemDto>();

			foreach (var item in lineupItems)
			{
				if (item is not null && Languages.TryParse(request.CountryCode, out var lang))
				{
					string description = await _translationRepo.GetTranslationAsync(item.DescriptionTranslation, lang.Id, cancellationToken);
					var mapped = item.MapLineupItem(description);

					if (mapped is not null)
					{
						result.Add(mapped);
					}
				}
			}

			return result;
		}
	}
}
