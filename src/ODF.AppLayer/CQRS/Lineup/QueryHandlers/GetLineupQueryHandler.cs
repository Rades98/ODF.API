﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ODF.AppLayer.CQRS.Lineup.Queries;
using ODF.AppLayer.Dtos;
using ODF.AppLayer.Mediator;
using ODF.AppLayer.Repos;
using ODF.Domain;
using ODF.Domain.Entities;

namespace ODF.AppLayer.CQRS.Lineup.QueryHandlers
{
	internal class GetLineupQueryHandler : IQueryHandler<GetLineupQuery, IEnumerable<LineupItemDto>>
	{
		private readonly ITranslationRepo _translationRepo;
		private readonly ILineupRepo _lineupRepo;
		private readonly ILogger<GetLineupQueryHandler> _logger;

		public GetLineupQueryHandler(ITranslationRepo translationRepo, ILineupRepo lineupRepo, ILogger<GetLineupQueryHandler> logger)
		{
			_lineupRepo = lineupRepo ?? throw new ArgumentNullException(nameof(lineupRepo));
			_translationRepo = translationRepo ?? throw new ArgumentNullException(nameof(translationRepo));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<IEnumerable<LineupItemDto>> Handle(GetLineupQuery request, CancellationToken cancellationToken)
		{
			var lineupItems = await _lineupRepo.GetLineupAsync(cancellationToken);

			var result = new List<LineupItemDto>();

			foreach (var item in lineupItems)
			{
				var mapped = await MapLineupItem(item, request.CountryCode, cancellationToken);
				if (mapped is not null)
				{
					result.Add(mapped);
				}
			}

			return result;
		}

		public async Task<LineupItemDto> MapLineupItem(LineupItem lineupItem, string countryCode, CancellationToken cancellationToken)
		{
			if (lineupItem is not null && Languages.TryParse(countryCode, out var lang))
			{
				string description = await _translationRepo.GetTranslationAsync(lineupItem.DescriptionTranslation, lang.Id, cancellationToken);

				if (!string.IsNullOrEmpty(description))
				{
					return new()
					{
						Id = lineupItem.Id,
						DateTime = lineupItem.DateTime,
						Description = description,
						Interpret = lineupItem.Interpret,
						PerformanceName = lineupItem.PerformanceName,
						Place = lineupItem.Place,
						DescriptionTranslationCode = lineupItem.DescriptionTranslation,
						UserName = lineupItem.UserName,
					};
				}

				_logger.LogWarning("Lineup item for {interpret} not found", lineupItem.Interpret);

				return null;
			}

			_logger.LogWarning("Language {lang} not found", countryCode);

			return null;
		}
	}
}
