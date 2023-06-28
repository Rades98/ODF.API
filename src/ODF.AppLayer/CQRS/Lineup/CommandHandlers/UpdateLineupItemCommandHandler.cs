using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ODF.AppLayer.CQRS.Lineup.Commands;
using ODF.AppLayer.Dtos.Validation;
using ODF.AppLayer.Mediator;
using ODF.AppLayer.Repos;
using ODF.Domain;
using ODF.Domain.Entities;

namespace ODF.AppLayer.CQRS.Lineup.CommandHandlers
{
	internal class UpdateLineupItemCommandHandler : ICommandHandler<UpdateLineupItemCommand, ValidationDto>
	{
		private readonly ILineupRepo _repo;
		private readonly ITranslationRepo _translationRepo;
		private readonly ILogger<AddLIneupItemCommandHandler> _logger;

		public UpdateLineupItemCommandHandler(ILineupRepo repo, ITranslationRepo translationRepo, ILogger<AddLIneupItemCommandHandler> logger)
		{
			_repo = repo ?? throw new ArgumentNullException(nameof(repo));
			_translationRepo = translationRepo ?? throw new ArgumentNullException(nameof(translationRepo));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<ValidationDto> Handle(UpdateLineupItemCommand request, CancellationToken cancellationToken)
		{
			if (Languages.TryParse(request.CountryCode, out var lang))
			{
				var toUpdate = await _repo.GetAsync(request.Id, cancellationToken);
				string transCode = !string.IsNullOrEmpty(request.DescriptionTranslationCode) ? request.DescriptionTranslationCode : toUpdate.DescriptionTranslation;
				string descTest = await _translationRepo.GetTranslationAsync(transCode, lang.Id, cancellationToken);

				if (descTest is null || !string.IsNullOrEmpty(request.Description) && descTest != request.Description)
				{
					await _translationRepo.UpdateOrInsertTransaltionAsync(transCode, request.Description, lang.Id, cancellationToken);
				}

				_logger.LogInformation("Updating lineup item with {perfName} and {desc} id:'{id}'", request.PerformanceName, request.DescriptionTranslationCode, request.Id);

				var lineupItem = new LineupItem()
				{
					Id = request.Id,
					DateTime = request.DateTime ?? toUpdate.DateTime,
					DescriptionTranslation = request.DescriptionTranslationCode,
					Interpret = request.Interpret,
					PerformanceName = request.PerformanceName,
					Place = request.Place.Trim(),
					UserName = request.UserName,
					UserNote = request.UserNote
				};

				return new() { IsOk = await _repo.UpdateLineupItemAsync(lineupItem, cancellationToken) };
			}

			return ValidationDto.Invalid;
		}
	}
}
