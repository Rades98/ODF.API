using ODF.AppLayer.CQRS.Translations.Queries;
using ODF.AppLayer.Services;
using ODF.Enums;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Logging;
using ODF.Data.Contracts.Interfaces;
using System;
using ODF.ServiceLayer.Contracts.Models;
using ODF.ServiceLayer.Contracts.Mapping;
using System.Linq;

namespace ODF.ServiceLayer.Translations
{
	internal class TranslationServices : ITranslationService
	{
        private readonly ITranslationRepo _repo;
        private readonly ILogger<TranslationServices> _logger;

        public TranslationServices(ITranslationRepo repo, ILogger<TranslationServices> logger)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<string> GetTranslation(TranslationRequestModel request, CancellationToken cancellationToken)
        {
            if (Languages.TryParse(request.CountryCode, out var lang))
            {
                return await _repo.GetTranslationOrDefaultTextAsync(request.TranslationIdentifier, request.DefaultText, lang.Id, cancellationToken);
            }

            _logger.LogWarning("Language {lang} not found", request.CountryCode);

            return null;
        }

        public async Task<TranslationsModel> GetTranslations(TranslationsRequestModel request, CancellationToken cancellationToken)
        {
               if (Languages.TryParse(request.CountryCode, out var lang))
               {
                   return new()
                   {
                       Translations = (await _repo.GetPagedAsync(request.Size, request.Offset, lang.Id, cancellationToken)).ToModels(),
                       Count = await _repo.GetTranslationsCountAsync(lang.Id, cancellationToken)
                   };
               }

               return new() { Translations = Enumerable.Empty<TranslationModel>(), Count = 0 };
        }
    }
}

