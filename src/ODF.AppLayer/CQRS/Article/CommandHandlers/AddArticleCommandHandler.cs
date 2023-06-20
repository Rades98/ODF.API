using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ODF.AppLayer.CQRS.Article.Commands;
using ODF.AppLayer.Dtos.Validation;
using ODF.AppLayer.Mediator;
using ODF.AppLayer.Repos;
using ODF.Domain;

namespace ODF.AppLayer.CQRS.Article.CommandHandlers
{
	internal class AddArticleCommandHandler : ICommandHandler<AddArticleCommand, ValidationDto>
	{
		private readonly IArticleRepo _repo;
		private readonly ITranslationRepo _translationRepo;
		private readonly ILogger<AddArticleCommandHandler> _logger;

		public AddArticleCommandHandler(IArticleRepo repo, ITranslationRepo translationRepo, ILogger<AddArticleCommandHandler> logger)
		{
			_repo = repo ?? throw new ArgumentNullException(nameof(repo));
			_translationRepo = translationRepo ?? throw new ArgumentNullException(nameof(translationRepo));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<ValidationDto> Handle(AddArticleCommand request, CancellationToken cancellationToken)
		{
			if (Languages.TryParse(request.CountryCode, out var lang))
			{
				string title = await _translationRepo.GetTranslationOrDefaultTextAsync(request.TitleTransaltionCode, request.Title, lang.Id, cancellationToken);
				string text = await _translationRepo.GetTranslationOrDefaultTextAsync(request.TextTransaltionCode, request.Text, lang.Id, cancellationToken);

				_logger.LogInformation("Creating article with {titleTransCode} and {textTransCode}", request.TitleTransaltionCode, request.TextTransaltionCode);

				if (title is not null && text is not null)
				{
					return new() { IsOk = await _repo.AddArticleAsync(request.TitleTransaltionCode, request.TextTransaltionCode, request.PageId, request.ImageUri, cancellationToken) };
				}
			}

			_logger.LogWarning("Language {lang} not found", request.CountryCode);

			return ValidationDto.Invalid;
		}
	}
}
