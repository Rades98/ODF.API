using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using ODF.AppLayer.CQRS.Article.Queries;
using ODF.AppLayer.Dtos;
using ODF.AppLayer.Repos;
using ODF.Domain;

namespace ODF.AppLayer.CQRS.Article.QueryHandlers
{
	internal class GetArticleQueryHandler : IRequestHandler<GetArticleQuery, ArticleDto>
	{
		private readonly IArticleRepo _repo;
		private readonly ITranslationRepo _translationRepo;
		private readonly ILogger<GetArticleQueryHandler> _logger;

		public GetArticleQueryHandler(IArticleRepo repo, ITranslationRepo translationRepo, ILogger<GetArticleQueryHandler> logger)
		{
			_repo = repo ?? throw new ArgumentNullException(nameof(repo));
			_translationRepo = translationRepo ?? throw new ArgumentNullException(nameof(translationRepo));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<ArticleDto> Handle(GetArticleQuery request, CancellationToken cancellationToken)
		{
			var article = await _repo.GetArticleAsync(request.Id, cancellationToken);

			if (article is not null && Languages.TryParse(request.CountryCode, out var lang))
			{
				var title = await _translationRepo.GetTranslationAsync(article.TitleTranslationCode, lang.Id, cancellationToken);
				var text = await _translationRepo.GetTranslationAsync(article.TextTranslationCode, lang.Id, cancellationToken);

				_logger.LogInformation("Get article with {titleTransCode} and {textTransCode}", article.TitleTranslationCode, article.TextTranslationCode);

				if (title is not null && text is not null)
				{
					return new()
					{
						Id = article.Id,
						Title = title,
						Text = text,
						ImageUri = article.ImageUri
					};
				}

				return null;
			}

			_logger.LogWarning("Language {lang} not found", request.CountryCode);

			return null;
		}
	}
}
