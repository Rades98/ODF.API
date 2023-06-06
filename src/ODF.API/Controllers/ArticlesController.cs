using System.Data;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using ODF.API.Controllers.Base;
using ODF.API.FormFactories;
using ODF.API.Registration.SettingModels;
using ODF.API.RequestModels.Forms;
using ODF.API.ResponseModels.Articles;
using ODF.API.ResponseModels.Common;
using ODF.API.ResponseModels.Exceptions;
using ODF.API.Responses;
using ODF.AppLayer.Consts;
using ODF.AppLayer.CQRS.Article.Commands;
using ODF.AppLayer.CQRS.Article.Queries;
using ODF.AppLayer.Extensions;
using ODF.AppLayer.Services.Interfaces;
using ODF.Domain;

namespace ODF.API.Controllers
{
	public class ArticlesController : BaseController
	{
		public ArticlesController(IMediator mediator, IOptions<ApiSettings> apiSettings, IActionDescriptorCollectionProvider adcp, ITranslationsProvider translationsProvider) : base(mediator, apiSettings, adcp, translationsProvider)
		{
		}

		[HttpPut(Name = nameof(AddArticle))]
		[Authorize(Roles = UserRoles.Admin)]
		[ProducesResponseType(typeof(CreateArticleResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(typeof(UnauthorizedExceptionResponseModel), StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> AddArticle([FromBody] AddArticleRequestForm model, [FromRoute] string countryCode, CancellationToken cancellationToken)
		{
			bool result = await Mediator.Send(new AddArticleCommand(model.TitleTranslationCode, model.Title,
				model.TextTranslationCode, model.Text, model.PageId, countryCode, model.ImageUrl), cancellationToken);

			if (!result)
			{
				return CustomApiResponses.InternalServerError(new ExceptionResponseModel("Vyskytla se chyba při tvorbě článku"));
			}

			var responseModel = new CreateArticleResponseModel("Článek byl úspěšně přidán.",
				ArticleFormFactory.GetAddArticleForm(model.Title, model.TitleTranslationCode, model.Text, model.TitleTranslationCode, model.PageId, model.ImageUrl));

			responseModel.AddTitleDeTranslation = GetTranslateArticleTitleAction(model.Title, Languages.Deutsch.GetCountryCode());
			responseModel.AddTextDeTranslation = GetTranslateArticleTextAction(model.Text, Languages.Deutsch.GetCountryCode());

			responseModel.AddTitleEnTranslation = GetTranslateArticleTitleAction(model.Title, Languages.English.GetCountryCode());
			responseModel.AddTextEnTranslation = GetTranslateArticleTextAction(model.Text, Languages.English.GetCountryCode());

			return Ok(responseModel);
		}

		[HttpGet("{articleId}", Name = nameof(GetArticle))]
		[ProducesResponseType(typeof(GetArticleResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(NotFoundExceptionResponseModel), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetArticle([FromRoute] int articleId, [FromRoute] string countryCode, CancellationToken cancellationToken)
		{
			var translations = await TranslationsProvider.GetTranslationsAsync(countryCode, cancellationToken);
			var result = await Mediator.Send(new GetArticleQuery(articleId, countryCode));

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
		public async Task<IActionResult> GetArticles(int size, int offset, int pageId, [FromRoute] string countryCode)
		{
			var articles = await Mediator.Send(new GetArticlesQuery(offset * size, size, pageId, countryCode));

			var responseModel = new GetArticlesResponseModel();

			if (articles.Any())
			{
				responseModel.Articles = articles
					.Where(art => !string.IsNullOrEmpty(art.Text) && !string.IsNullOrEmpty(art.Title))
					.Select(art =>
					{
						var action = GetAppAction(nameof(GetArticle), "");
						action.Curl.Href = new(action.Curl.Href.ToString().Replace("{articleId}", $"{art.Id}"));
						return new GetArticleResponseModel(action.Curl.Href.ToString(), "article", action.Curl.Method, art.Title, art.Text, art.ImageUri);
					});
			}

			return Ok(responseModel);
		}

		private NamedAction GetTranslateArticleTextAction(string translationCode, string countryCode)
			=> GetNamedAction(nameof(TranslationsController.ChangeTranslation), $"Přeložit text do {countryCode}", "translate_article",
					TranslationFormFactory.GetChangeTranslationForm(translationCode, "", countryCode));

		private NamedAction GetTranslateArticleTitleAction(string translationCode, string countryCode)
			=> GetNamedAction(nameof(TranslationsController.ChangeTranslation), $"Přeložit nadpis do {countryCode}", "translate_article",
					TranslationFormFactory.GetChangeTranslationForm(translationCode, "", countryCode));
	}
}
