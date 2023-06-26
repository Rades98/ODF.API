using System;
using System.Threading;
using System.Threading.Tasks;
using ODF.AppLayer.CQRS.Translations.Commands;
using ODF.AppLayer.Dtos.Validation;
using ODF.AppLayer.Mediator;
using ODF.AppLayer.Repos;
using ODF.Domain;

namespace ODF.AppLayer.CQRS.Translations.CommandHandlers
{
	internal class UpdateTransaltionCommandHandler : ICommandHandler<UpdateTransaltionCommand, ValidationDto>
	{
		private readonly ITranslationRepo _repo;

		public UpdateTransaltionCommandHandler(ITranslationRepo repo)
		{
			_repo = repo ?? throw new ArgumentNullException(nameof(repo));
		}

		public async Task<ValidationDto> Handle(UpdateTransaltionCommand request, CancellationToken cancellationToken)
		{
			if (Languages.TryParse(request.CountryCode, out var lang))
			{
				return new() { IsOk = await _repo.UpdateOrInsertTransaltionAsync(request.TranslationCode, request.Text, lang.Id, cancellationToken) };
			}

			return ValidationDto.Invalid;
		}
	}
}
