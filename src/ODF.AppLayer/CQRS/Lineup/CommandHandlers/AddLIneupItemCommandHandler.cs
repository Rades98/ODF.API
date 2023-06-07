using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ODF.AppLayer.CQRS.Lineup.Commands;
using ODF.AppLayer.Mediator;
using ODF.AppLayer.Repos;
using ODF.Domain;
using ODF.Domain.Entities;

namespace ODF.AppLayer.CQRS.Lineup.CommandHandlers
{
	internal class AddLIneupItemCommandHandler : ICommandHandler<AddLineupItemCommand, bool>
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
				var descTest = await _translationRepo.GetTranslationAsync(request.DescriptionTranslationCode, lang.Id, cancellationToken);

				if (!string.IsNullOrEmpty(descTest))
				{
					return false;
				}

				var desc = await _translationRepo.GetTranslationOrDefaultTextAsync(request.DescriptionTranslationCode, request.Description, lang.Id, cancellationToken);

				_logger.LogInformation("Creating lineup item with {perfName} and {desc}", request.PerformanceName, request.DescriptionTranslationCode);

				if (desc is not null)
				{
					var lineupItem = new LineupItem()
					{
						DateTime = request.DateTime,
						DescriptionTranslation = request.DescriptionTranslationCode,
						Interpret = request.Interpret,
						PerformanceName = request.PerformanceName,
						Place = request.Place.Trim(),
					};

					return await _repo.AddLineupItemAsync(lineupItem, cancellationToken);
				}
			}

			_logger.LogWarning("Language {lang} not found", request.CountryCode);

			return false;
		}
	}
}
