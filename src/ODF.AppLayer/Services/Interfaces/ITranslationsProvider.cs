using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ODF.AppLayer.Dtos;

namespace ODF.AppLayer.Services.Interfaces
{
	public interface ITranslationsProvider
	{
		public Task<IReadOnlyList<TranslationDto>> GetTranslationsAsync(string countryCode, CancellationToken cancellationToken);
	}
}
