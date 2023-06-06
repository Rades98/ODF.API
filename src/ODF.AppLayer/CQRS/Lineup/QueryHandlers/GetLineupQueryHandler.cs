using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using ODF.AppLayer.CQRS.Article.QueryHandlers;
using ODF.AppLayer.CQRS.Lineup.Queries;
using ODF.AppLayer.Dtos;
using ODF.AppLayer.Repos;
using ODF.Domain;
using ODF.Domain.Entities;

namespace ODF.AppLayer.CQRS.Lineup.QueryHandlers
{
	internal class GetLineupQueryHandler : IRequestHandler<GetLineupQuery, IEnumerable<LineupItemDto>>
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
				if(mapped is not null)
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
				var description = await _translationRepo.GetTranslationAsync(lineupItem.DescriptionTranslation, lang.Id, cancellationToken);

				if(!string.IsNullOrEmpty(description))
				{
					return new()
					{
						DateTime = lineupItem.DateTime,
						Description = description,
						Interpret = lineupItem.Interpret,
						PerformanceName = lineupItem.PerformanceName,
						Place = lineupItem.Place,
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
