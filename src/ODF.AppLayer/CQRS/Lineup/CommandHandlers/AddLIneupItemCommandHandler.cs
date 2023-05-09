using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using ODF.AppLayer.CQRS.Article.CommandHandlers;
using ODF.AppLayer.CQRS.Lineup.Commands;
using ODF.Data.Contracts.Entities;
using ODF.Data.Contracts.Interfaces;
using ODF.Enums;

namespace ODF.AppLayer.CQRS.Lineup.CommandHandlers
{
	internal class AddLIneupItemCommandHandler : IRequestHandler<AddLineupItemCommand, bool>
	{
		private readonly ILineupRepo _repo;
		private readonly ITranslationRepo _translationRepo;
		private readonly ILogger<AddLIneupItemCommandHandler> _logger;

		public AddLIneupItemCommandHandler(ILineupRepo repo, ITranslationRepo translationRepo, ILogger<AddLIneupItemCommandHandler> logger)
		{
			_repo = repo ?? throw new ArgumentNullException(nameof(repo));
			_translationRepo = translationRepo ?? throw new ArgumentNullException(nameof(translationRepo));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<bool> Handle(AddLineupItemCommand request, CancellationToken cancellationToken)
		{
			if (Languages.TryParse(request.CountryCode, out var lang))
			{
				var perfNameTest = await _translationRepo.GetTranslationAsync(request.PerformanceNameTranslationCode, lang.Id, cancellationToken);
				var descTest = await _translationRepo.GetTranslationAsync(request.DescriptionTranslationCode, lang.Id, cancellationToken);

				if(perfNameTest is not null || descTest is not null)
				{
					return false;
				}

				var perfName = await _translationRepo.GetTranslationOrDefaultTextAsync(request.PerformanceNameTranslationCode, request.PerformanceName, lang.Id, cancellationToken);
				var desc = await _translationRepo.GetTranslationOrDefaultTextAsync(request.DescriptionTranslationCode, request.Description, lang.Id, cancellationToken);

				_logger.LogInformation("Creating lineup item with {perfName} and {desc}", request.PerformanceNameTranslationCode, request.DescriptionTranslationCode);

				if (desc is not null && perfName is not null)
				{
					var lineupItem = new LineupItem()
					{
						DateTime = request.DateTime,
						DescriptionTranslation = request.DescriptionTranslationCode,
						Interpret = request.Interpret,
						PerformanceNameTranslation = request.PerformanceNameTranslationCode,
						Place = request.Place,
					};

					return await _repo.AddLineupItemAsync(lineupItem, cancellationToken);
				}
			}

			_logger.LogWarning("Language {lang} not found", request.CountryCode);

			return false;
		}
	}
}
