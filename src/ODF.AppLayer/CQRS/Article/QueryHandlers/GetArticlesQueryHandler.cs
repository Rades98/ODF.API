﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ODF.AppLayer.CQRS.Article.Queries;
using ODF.AppLayer.Dtos;
using ODF.AppLayer.Mediator;
using ODF.AppLayer.Repos;
using ODF.Domain;

namespace ODF.AppLayer.CQRS.Article.QueryHandlers
{
	internal class GetArticlesQueryHandler : IQueryHandler<GetArticlesQuery, IEnumerable<ArticleDto>>
	{
		private readonly IArticleRepo _repo;
		private readonly ITranslationRepo _translationRepo;
		private readonly ILogger<GetArticlesQueryHandler> _logger;

		public GetArticlesQueryHandler(IArticleRepo repo, ITranslationRepo translationRepo, ILogger<GetArticlesQueryHandler> logger)
		{
			_repo = repo ?? throw new ArgumentNullException(nameof(repo));
			_translationRepo = translationRepo ?? throw new ArgumentNullException(nameof(translationRepo));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<IEnumerable<ArticleDto>> Handle(GetArticlesQuery request, CancellationToken cancellationToken)
		{
			var articles = await _repo.GetArticlesPaginatedAsync(request.PageId, request.Size, request.Offset, cancellationToken);

			var result = new List<ArticleDto>();

			foreach (var article in articles)
			{
				result.Add(await MapArticle(article, request.CountryCode, cancellationToken));
			}

			return result;
		}

		private async Task<ArticleDto> MapArticle(Domain.Entities.Article article, string countryCode, CancellationToken cancellationToken)
		{
			if (article is not null && Languages.TryParse(countryCode, out var lang))
			{
				string title = await _translationRepo.GetTranslationAsync(article.TitleTranslationCode, lang.Id, cancellationToken);
				string text = await _translationRepo.GetTranslationAsync(article.TextTranslationCode, lang.Id, cancellationToken);

				if (title is not null && text is not null)
				{
					return new()
					{
						Id = article.Id,
						Title = title,
						Text = text,
						ImageUri = new(article.ImageUri)
					};
				}

				_logger.LogWarning("Article with title: {titleTranslationCode} not found", article.TitleTranslationCode);

				return null;
			}

			_logger.LogWarning("Language {lang} not found", countryCode);

			return null;
		}
	}
}
