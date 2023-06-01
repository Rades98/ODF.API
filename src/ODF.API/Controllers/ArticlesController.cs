using System.Data;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
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
using ODF.AppLayer.CQRS.Translations.Queries;
using ODF.Enums;

namespace ODF.API.Controllers
{
	public class ArticlesController : Controller
	{
		private readonly IMediator _mediator;
		private readonly ApiSettings _settings;

		public ArticlesController(IMediator mediator, IOptions<ApiSettings> apiSettings)
		{
			_mediator = mediator;
			_settings = apiSettings.Value;
		}


		/* 2RADEK: IResult vs IActionResult. Nerozumiem, preco CustomApiResponses vracaju IResult a nie IActionResult. IActionResult ma predsalen podstatne viac stavovych hodnot, nie? Bolo nutne to pretypovat. */
		/* 2RADEK: Ked uz sa snazime o HATEOAS, nemali by sme vratit aj idecko vlozeneho clanku? */
		[Authorize(Roles = UserRoles.Admin)]
		[HttpPut("/{countryCode}/articles")]
		[ProducesResponseTypeAttribute(typeof(CreateArticleResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseTypeAttribute(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
		[ProducesResponseTypeAttribute(typeof(UnauthorizedExceptionResponseModel), StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> AddArticle([FromBody] AddArticleRequestForm model, [FromRoute] string countryCode, CancellationToken cancellationToken)
		{
			bool result = await _mediator.Send(new AddArticleCommand(model.TitleTranslationCode, model.Title, model.TextTranslationCode, model.Text, model.PageId, countryCode, model.ImageUrl), cancellationToken);

			if (!result)
			{
				return (IActionResult)CustomApiResponses.InternalServerError(new ExceptionResponseModel("Vyskytla se chyba při tvorbě článku"));
			}

			var responseModel = new CreateArticleResponseModel(_settings.ApiUrl, "Článek byl úspěšně přidán.",
				ArticleFormFactory.GetAddArticleForm(model.Title, model.TitleTranslationCode, model.Text, model.TitleTranslationCode, model.PageId, model.ImageUrl));

			responseModel.AddTitleDeTranslation = GetTranslateArticleTitleAction(_settings.ApiUrl, model.Title, Languages.Deutsch.GetCountryCode());
			responseModel.AddTextDeTranslation = GetTranslateArticleTextAction(_settings.ApiUrl, model.Text, Languages.Deutsch.GetCountryCode());

			responseModel.AddTitleEnTranslation = GetTranslateArticleTitleAction(_settings.ApiUrl, model.Title, Languages.English.GetCountryCode());
			responseModel.AddTextEnTranslation = GetTranslateArticleTextAction(_settings.ApiUrl, model.Text, Languages.English.GetCountryCode());

			return Ok(responseModel);
		}

		/* 2RADEK: Dovoli mi vlozit clanok s blbostou v URL, ale pri nacitani mi to rupne, ze to nie je platna url. Nutna validacia?*/
		[HttpGet("/{countryCode}/articles/{articleId}")]
		[ProducesResponseType(typeof(GetArticleResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(NotFoundExceptionResponseModel), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetArticle([FromRoute] int articleId, [FromRoute] string countryCode)
		{
			var result = await _mediator.Send(new GetArticleQuery(articleId, countryCode));

			if (result != null)
			{
				var responseModel = new GetArticleResponseModel(_settings.ApiUrl, articleId, countryCode, result.Title, result.Text, result.ImageUri);

				return Ok(responseModel);
			}

			string notFoundTitle = await _mediator.Send(new GetTranslationQuery("Zdroj nenalezen.", "app_base_notfound", countryCode));
			string notFoundArticle = await _mediator.Send(new GetTranslationQuery("Článek, který se pokoušíte zobrazit, byl nejspíše smazán.", "app_article_notfound", countryCode));

			return NotFound(new NotFoundExceptionResponseModel(notFoundTitle, notFoundArticle));
		}

		[HttpGet("/{countryCode}/articles")]
		[ProducesResponseType(typeof(GetArticleResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetArticles(int size, int offset, int pageId, [FromRoute] string countryCode)
		{
			var articles = await _mediator.Send(new GetArticlesQuery(offset * size, size, pageId, countryCode));

			var responseModel = new GetArticlesResponseModel(_settings.ApiUrl, size, offset, pageId, countryCode);

			if (articles.Any())
			{
				responseModel.Articles = articles
					.Where(art => !string.IsNullOrEmpty(art.Text) && !string.IsNullOrEmpty(art.Title))
					.Select(art => new GetArticleResponseModel(_settings.ApiUrl, art.Id, countryCode, art.Title, art.Text, art.ImageUri));
			}

			return Ok(responseModel);
		}

		private static NamedAction GetTranslateArticleTextAction(string baseUrl, string translationCode, string countryCode)
			=> new($"{baseUrl}/{countryCode}/articles", $"Přeložit text do {countryCode}", "translate_article", HttpMethods.Put,
					TranslationFormFactory.GetChangeTranslationForm(translationCode, "", countryCode));

		private static NamedAction GetTranslateArticleTitleAction(string baseUrl, string translationCode, string countryCode)
			=> new($"{baseUrl}/{countryCode}/articles", $"Přeložit nadpis do {countryCode}", "translate_article", HttpMethods.Put,
					TranslationFormFactory.GetChangeTranslationForm(translationCode, "", countryCode));
	}
}
