﻿using System.Data;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using ODF.API.Attributes.HtttpMethodAttributes;
using ODF.API.Controllers.Base;
using ODF.API.FormComposers;
using ODF.API.RequestModels.Forms;
using ODF.API.ResponseModels.Articles;
using ODF.API.ResponseModels.Common;
using ODF.API.ResponseModels.Contacts.Create;
using ODF.API.ResponseModels.Exceptions;
using ODF.AppLayer.CQRS.Article.Commands;
using ODF.AppLayer.CQRS.Article.Queries;
using ODF.AppLayer.Dtos;
using ODF.AppLayer.Extensions;
using ODF.AppLayer.Services.Interfaces;
using ODF.Domain;
using ODF.Domain.Constants;
using ODF.Domain.SettingModels;

namespace ODF.API.Controllers
{
	public class ArticleController : BaseController
	{
		public ArticleController(IMediator mediator, IOptions<ApiSettings> apiSettings, IActionDescriptorCollectionProvider adcp, ITranslationsProvider translationsProvider)
			: base(mediator, apiSettings, adcp, translationsProvider)
		{
		}

		[HttpPost(Name = nameof(AddArticle))]
		[Authorize(Roles = UserRoles.Admin)]
		[CountryCodeFilter("cz")]
		[ProducesResponseType(typeof(CreateArticleResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(typeof(UnauthorizedExceptionResponseModel), StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> AddArticle([FromBody] AddArticleRequestForm requestForm, CancellationToken cancellationToken)
		{
			var validationResult = await Mediator.Send(new AddArticleCommand(requestForm, CountryCode), cancellationToken);

			if (validationResult.IsOk)
			{
				var responseModel = new CreateArticleResponseModel("Článek byl úspěšně přidán.", ArticleFormComposer.GetAddArticleForm(requestForm));

				responseModel.AddTitleDeTranslation = GetTranslateArticleTitleAction(requestForm.Title, Languages.Deutsch.GetCountryCode());
				responseModel.AddTextDeTranslation = GetTranslateArticleTextAction(requestForm.Text, Languages.Deutsch.GetCountryCode());

				responseModel.AddTitleEnTranslation = GetTranslateArticleTitleAction(requestForm.Title, Languages.English.GetCountryCode());
				responseModel.AddTextEnTranslation = GetTranslateArticleTextAction(requestForm.Text, Languages.English.GetCountryCode());

				return Ok(responseModel);
			}

			if (validationResult.Errors.Any())
			{
				var responseForm = ArticleFormComposer.GetAddArticleForm(requestForm, validationResult.Errors);
				return UnprocessableEntity(new CreateContactPersonResponseModel(responseForm));
			}

			return InternalServerError(new ExceptionResponseModel("Vyskytla se chyba při tvorbě článku"));
		}

		[HttpGet("{articleId}", Name = nameof(GetArticle))]
		[ProducesResponseType(typeof(GetArticleResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(NotFoundExceptionResponseModel), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetArticle([FromRoute] int articleId, CancellationToken cancellationToken)
		{
			var translations = await TranslationsProvider.GetTranslationsAsync(CountryCode, cancellationToken);
			var result = await Mediator.Send(new GetArticleQuery(articleId, CountryCode), cancellationToken);

			if (result != null)
			{
				var responseModel = new GetArticleResponseModel(result.Title, result.Text, result.ImageUri);

				return Ok(responseModel);
			}

			return NotFound(new NotFoundExceptionResponseModel(translations.Get("app_base_notfound"), translations.Get("app_article_notfound")));
		}

		[HttpGet(Name = nameof(GetArticles))]
		[ProducesResponseType(typeof(GetArticleResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetArticles(int size, int offset, int pageId, CancellationToken cancellationToken)
		{
			var articles = await Mediator.Send(new GetArticlesQuery(offset * size, size, pageId, CountryCode), cancellationToken);

			var responseModel = new GetArticlesResponseModel();

			if (articles.Any())
			{
				responseModel.Articles = articles
					.Where(art => !string.IsNullOrEmpty(art.Text) && !string.IsNullOrEmpty(art.Title))
					.Select(MapArticle);
			}

			return Ok(responseModel);
		}

		private NamedAction GetTranslateArticleTextAction(string translationCode, string countryCode)
			=> GetNamedAction(nameof(TranslationsController.ChangeTranslation), $"Přeložit text do {countryCode}", "translate_article",
					TranslationFormComposer.GetChangeTranslationForm(new() { TranslationCode = translationCode }));

		private NamedAction GetTranslateArticleTitleAction(string translationCode, string countryCode)
			=> GetNamedAction(nameof(TranslationsController.ChangeTranslation), $"Přeložit nadpis do {countryCode}", "translate_article",
					TranslationFormComposer.GetChangeTranslationForm(new() { TranslationCode = translationCode }));

		private GetArticleResponseModel MapArticle(ArticleDto art)
		{
			var action = GetAppAction(nameof(GetArticle), "");
			action.Curl.Href = new(action.Curl.Href.ToString().Replace("{articleId}", $"{art.Id}"));
			return new GetArticleResponseModel(action.Curl.Href.ToString(), "article", action.Curl.Method, art.Title, art.Text, art.ImageUri);
		}
	}
}
