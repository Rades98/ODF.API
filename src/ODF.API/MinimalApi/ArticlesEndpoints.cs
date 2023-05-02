using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

namespace ODF.API.MinimalApi
{
	public static class ArticlesEndpoints
	{
		public static WebApplication MapArticlesEndpoints(this WebApplication app, IMediator mediator, ApiSettings apiSettings)
		{
			app.MapPut("/articles", [Authorize(Roles = UserRoles.Admin)] async ([FromBody] AddArticleRequestForm model, CancellationToken cancellationToken) =>
			{
				var result = await mediator.Send(new AddArticleCommand(model.TitleTranslationCode, model.Title, model.TextTranslationCode, model.Text, model.PageId, model.CountryCode, model.ImageUrl), cancellationToken);

				if (!result)
				{
					return CustomApiResponses.InternalServerError(new ExceptionResponseModel("Vyskytla se chyba při tvorbě článku"));
				}

				var responseModel = new PutArticleResponseModel(apiSettings.ApiUrl, "Článek byl úspěšně přidán.",
					ArticleFormFactory.GetAddArticleForm(model.Title, model.TitleTranslationCode, model.Text, model.TitleTranslationCode, model.PageId, model.CountryCode, model.ImageUrl));

				responseModel.AddTitleDeTranslation = GetTranslateArticleTitleAction(apiSettings.ApiUrl, model.Title, Languages.Deutsch.GetCountryCode());
				responseModel.AddTextDeTranslation = GetTranslateArticleTextAction(apiSettings.ApiUrl, model.Text, Languages.Deutsch.GetCountryCode());

				responseModel.AddTitleEnTranslation = GetTranslateArticleTitleAction(apiSettings.ApiUrl, model.Title, Languages.English.GetCountryCode());
				responseModel.AddTextEnTranslation = GetTranslateArticleTextAction(apiSettings.ApiUrl, model.Text, Languages.English.GetCountryCode());

				return Results.Ok(responseModel);
			})
			.WithMetadata(new ProducesResponseTypeAttribute(typeof(PutArticleResponseModel), StatusCodes.Status200OK))
			.WithMetadata(new ProducesResponseTypeAttribute(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError))
			.WithMetadata(new ProducesResponseTypeAttribute(typeof(UnauthorizedExceptionResponseModel), StatusCodes.Status401Unauthorized));

			app.MapGet("/{countryCode}/articles/{articleId}", async ([FromRoute] int articleId, [FromRoute] string countryCode, CancellationToken cancellationToken) =>
			{
				var result = await mediator.Send(new GetArticleQuery(articleId, countryCode), cancellationToken);
				
				if (result is not null)
				{
					var responseModel = new GetArticleResponseModel(apiSettings.ApiUrl, articleId, countryCode, result.Title, result.Text, result.ImageUri);

					return Results.Ok(responseModel);
				}

				var notFoundTitle = await mediator.Send(new GetTranslationQuery("Zdroj nenalezen.", "app_base_notfound", countryCode), cancellationToken);
				var notFoundArticle = await mediator.Send(new GetTranslationQuery("Článek, který se pokoušíte zobrazit, byl nejspíše smazán.", "app_article_notfound", countryCode), cancellationToken);

				return CustomApiResponses.NotFound(new NotFoundExceptionResponseModel(notFoundTitle, notFoundArticle));
				
			})
			.WithMetadata(new ProducesResponseTypeAttribute(typeof(GetArticleResponseModel), StatusCodes.Status200OK))
			.WithMetadata(new ProducesResponseTypeAttribute(typeof(NotFoundExceptionResponseModel), StatusCodes.Status404NotFound))
			.WithMetadata(new ProducesResponseTypeAttribute(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError));

			app.MapGet("/{countryCode}/articles", async ([FromQuery] int size, [FromQuery] int offset, [FromQuery] int pageId, [FromRoute] string countryCode, CancellationToken cancellationToken) =>
			{
				var articles = await mediator.Send(new GetArticlesQuery(offset * size, size, pageId, countryCode), cancellationToken);

				var responseModel = new GetArticlesResponseModel(apiSettings.ApiUrl, size, offset, pageId, countryCode);

				if(articles.Any())
				{
					responseModel.Articles = articles.Select(art => new GetArticleResponseModel(apiSettings.ApiUrl, art.Id, countryCode, art.Title, art.Text, art.ImageUri));
				}

				return responseModel;
			})
			.WithMetadata(new ProducesResponseTypeAttribute(typeof(GetArticleResponseModel), StatusCodes.Status200OK))
			.WithMetadata(new ProducesResponseTypeAttribute(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError));

			return app;
		}

		private static NamedAction GetTranslateArticleTextAction(string baseUrl, string translationCode, string countryCode)
			=> new NamedAction($"{baseUrl}/articles", $"Přeložit text do {countryCode}", "translate_article", HttpMethods.Put,
					TranslationFormFactory.GetChangeTranslationForm(translationCode, "", countryCode));

		private static NamedAction GetTranslateArticleTitleAction(string baseUrl, string translationCode, string countryCode)
			=> new NamedAction($"{baseUrl}/articles", $"Přeložit nadpis do {countryCode}", "translate_article", HttpMethods.Put,
					TranslationFormFactory.GetChangeTranslationForm(translationCode, "", countryCode));
	}
}
