using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using ODF.AppLayer.CQRS.Translations.Queries;
using ODF.Data.Contracts.Interfaces;
using ODF.Enums;

namespace ODF.AppLayer.CQRS.Translations.QueryHandlers
{
	internal class GetTranslationQueryHandler : IRequestHandler<GetTranslationQuery, string>
	{
		private readonly ITranslationRepo _repo;
		private readonly ILogger<GetTranslationQueryHandler> _logger;

		public GetTranslationQueryHandler(ITranslationRepo repo, ILogger<GetTranslationQueryHandler> logger)
		{
			_repo = repo ?? throw new ArgumentNullException(nameof(repo));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<string> Handle(GetTranslationQuery request, CancellationToken cancellationToken)
		{
			if (Languages.TryParse(request.CountryCode, out var lang))
			{
				return await _repo.GetTranslationOrDefaultTextAsync(request.TranslationIdentifier, request.DefaultText, lang.Id, cancellationToken);
			}

			_logger.LogWarning("Language {lang} not found", request.CountryCode);

			return null;
		}
	}
}
