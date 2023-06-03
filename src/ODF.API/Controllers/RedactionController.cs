using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ODF.API.Controllers.Base;
using ODF.API.FormFactories;
using ODF.API.Registration.SettingModels;
using ODF.API.ResponseModels.Common;
using ODF.API.ResponseModels.Exceptions;
using ODF.API.ResponseModels.Redaction;
using ODF.AppLayer.Consts;
using ODF.AppLayer.CQRS.Translations.Queries;
using ODF.Enums;

namespace ODF.API.Controllers
{
	public class RedactionController : BaseController
	{
		public RedactionController(IMediator mediator, IOptions<ApiSettings> apiSettings) : base(mediator, apiSettings)
		{
		}

		[HttpGet(Name = nameof(GetRedaction))]
		[Authorize(Roles = UserRoles.Admin)]
		[ProducesResponseType(typeof(RedactionResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(typeof(UnauthorizedExceptionResponseModel), StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> GetRedaction([FromRoute] string countryCode, CancellationToken cancellationToken)
		{
			if (countryCode.ToUpper() != Languages.Czech.GetCountryCode())
			{
				return UnprocessableEntity("This action is supported for CZ language only");
			}

			var responseModel = new RedactionResponseModel(ApiSettings.ApiUrl, countryCode, "Redakce");

			string aboutTranslation = await Mediator.Send(new GetTranslationQuery("O festivalu", "nav_about", countryCode), cancellationToken);
			responseModel.AddAboutArticle = GetAddArticleAction(ApiSettings.ApiUrl, aboutTranslation, 0, countryCode);

			string associationTranslation = await Mediator.Send(new GetTranslationQuery("FolklorOVA", "nav_association", countryCode), cancellationToken);
			responseModel.AddAssociationArticle = GetAddArticleAction(ApiSettings.ApiUrl, associationTranslation, 1, countryCode);

			responseModel.AddLineupItem = GetAddLineupAction(ApiSettings.ApiUrl, countryCode);
			responseModel.UpdateContacts = new NamedAction($"{ApiSettings.ApiUrl}/{countryCode}/contacts/redaction", "Upravit kontakty", "updateContacts", HttpMethods.Get);

			responseModel.AddAction($"/{countryCode}/translations?size=20&offset=0", "translations_change", HttpMethods.Get);

			return Ok(responseModel);
		}

		private static NamedAction GetAddArticleAction(string baseUrl, string sectionTranslation, int pageNum, string countryCode)
				=> new($"{baseUrl}/{countryCode}/articles", $"Přidat článek do {sectionTranslation}", "add_article", HttpMethods.Put,
						ArticleFormFactory.GetAddArticleForm("", $"page{pageNum}_title_{{id}}", "", $"page{pageNum}_text_{{id}}", pageNum));

		private static NamedAction GetAddLineupAction(string baseUrl, string countryCode)
			=> new($"{baseUrl}/{countryCode}/lineup", $"Přidat item do programu", "add_lineup_item", HttpMethods.Put,
					LineupItemFormFactory.GetAddLineupItemForm("Místo", "Interpret", "Název představení", "popis vystoupení", "{vystoupeni}_desc", DateTime.Now));
	}
}
