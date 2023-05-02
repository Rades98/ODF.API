using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ODF.AppLayer.CQRS.Translations.Commands;
using ODF.Data.Contracts.Interfaces;
using ODF.Enums;

namespace ODF.AppLayer.CQRS.Translations.CommandHandlers
{
	internal class ModifyTransaltionCommandHandler : IRequestHandler<ModifyTransaltionCommand, bool>
	{
		private readonly ITranslationRepo _repo;

		public ModifyTransaltionCommandHandler(ITranslationRepo repo)
		{
			_repo = repo ?? throw new ArgumentNullException(nameof(repo));
		}


		public async Task<bool> Handle(ModifyTransaltionCommand request, CancellationToken cancellationToken)
		{
			if (Languages.TryParse(request.CountryCode, out var lang))
			{
				return await _repo.UpdateOrInsertTransaltionAsync(request.TranslationCode, request.Text, lang.Id, cancellationToken);
			}

			return false;
		}
	}
}
