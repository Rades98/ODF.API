using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ODF.Data.Contracts.Entities;

namespace ODF.Data.Contracts.Interfaces
{
	public interface ITranslationRepo
	{
		Task<string> GetTranslationOrDefaultTextAsync(string translationIdentifier, string defaultTranslation, int languageId, CancellationToken cancellationToken);

		Task<string> GetTranslationAsync(string translationIdentifier, int languageId, CancellationToken cancellationToken);

		Task<bool> AddTranslationAsync(string translationIdentifier, string text, int languageId, CancellationToken cancellationToken);

		Task<IEnumerable<Translation>> GetPagedAsync(int size, int offset, int languageId, CancellationToken cancellationToken);

		Task<bool> UpdateOrInsertTransaltionAsync(string translationIdentifier, string text, int languageId, CancellationToken cancellationToken);

		Task<long> GetTranslationsCountAsync(int languageId, CancellationToken cancellationToken);
	}
}
