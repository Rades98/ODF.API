using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ODF.AppLayer.CQRS.Lineup.Commands;
using ODF.AppLayer.Dtos.Validation;
using ODF.AppLayer.Mediator;
using ODF.AppLayer.Repos;

namespace ODF.AppLayer.CQRS.Lineup.CommandHandlers
{
	internal class DeleteLineupItemCommandHandler : ICommandHandler<DeleteLineupItemCommand, ValidationDto>
	{
		private readonly ILineupRepo _repo;
		private readonly ILogger _logger;
		private readonly ITranslationRepo _translationRepo;

		public DeleteLineupItemCommandHandler(ILineupRepo repo, ILogger<AddLIneupItemCommandHandler> logger, ITranslationRepo transl)
		{
			_repo = repo ?? throw new ArgumentNullException(nameof(repo));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_translationRepo = transl ?? throw new ArgumentNullException(nameof(transl));
		}

		public async Task<ValidationDto> Handle(DeleteLineupItemCommand request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Deleting lineup item with id {id}", request.Id);

			var toRemove = await _repo.GetAsync(request.Id, cancellationToken);

			if (await _repo.RemoveLineupItemAsync(request.Id, cancellationToken))
			{
				return new() { IsOk = await _translationRepo.DeleteTranslationAsync(toRemove.DescriptionTranslation, cancellationToken) };
			}

			return ValidationDto.Invalid;

		}
	}
}
