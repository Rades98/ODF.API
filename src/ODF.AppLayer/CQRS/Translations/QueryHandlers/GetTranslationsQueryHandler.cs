using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ODF.AppLayer.CQRS.Translations.Queries;
using ODF.AppLayer.Dtos;
using ODF.AppLayer.Mapping;
using ODF.Data.Contracts.Interfaces;
using ODF.Enums;

namespace ODF.AppLayer.CQRS.Translations.QueryHandlers
{
	internal class GetTranslationsQueryHandler : IRequestHandler<GetTranslationsQuery, TranslationsDto>
	{
		private readonly ITranslationRepo _repo;

		public GetTranslationsQueryHandler(ITranslationRepo repo)
		{
			_repo = repo ?? throw new ArgumentNullException(nameof(repo));
		}

		public async Task<TranslationsDto> Handle(GetTranslationsQuery request, CancellationToken cancellationToken)
		{
			if (Languages.TryParse(request.CountryCode, out var lang))
			{
				return new()
				{
					Translations = (await _repo.GetPagedAsync(request.Size, request.Offset, lang.Id, cancellationToken)).ToDtos(),
					Count = await _repo.GetTranslationsCountAsync(lang.Id, cancellationToken)
				};
			}

			return new() { Translations = Enumerable.Empty<TranslationDto>(), Count = 0 };
		}
	}
}
