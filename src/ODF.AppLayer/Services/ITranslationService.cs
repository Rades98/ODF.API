using ODF.AppLayer.CQRS.Translations.Queries;
using System.Threading.Tasks;
using System.Threading;
using ODF.ServiceLayer.Contracts.Models;

namespace ODF.AppLayer.Services
{
	public interface ITranslationService
	{
        public Task<string> GetTranslation(TranslationRequestModel request,  CancellationToken cancellationToken);

        public Task<TranslationsModel> GetTranslations(TranslationsRequestModel request, CancellationToken cancellationToken);
        
        //Get translation

        //Get list of all translations

        //Add transaltion

        //Remove translation
    }
}
