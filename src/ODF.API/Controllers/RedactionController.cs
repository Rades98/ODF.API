using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using ODF.API.Controllers.Base;
using ODF.API.Controllers.Contacts;
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
		public RedactionController(IMediator mediator, IOptions<ApiSettings> apiSettings, IActionDescriptorCollectionProvider adcp) : base(mediator, apiSettings, adcp)
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

			var responseModel = new RedactionResponseModel("Redakce");

			string aboutTranslation = await Mediator.Send(new GetTranslationQuery("O festivalu", "nav_about", countryCode), cancellationToken);
			responseModel.AddAboutArticle = GetAddArticleAction(ApiBaseUrl, aboutTranslation, 0, countryCode); // TODO ADD FORM

			string associationTranslation = await Mediator.Send(new GetTranslationQuery("FolklorOVA", "nav_association", countryCode), cancellationToken);
			responseModel.AddAssociationArticle = GetAddArticleAction(ApiBaseUrl, associationTranslation, 1, countryCode); // TODO ADD FORM

			responseModel.AddLineupItem = GetNamedAction(nameof(LineupController.AddItemToLineup), $"Přidat item do programu", "add_lineup_item",
					LineupItemFormFactory.GetAddLineupItemForm("Místo", "Interpret", "Název představení", "popis vystoupení", "{vystoupeni}_desc", DateTime.Now));

			responseModel.UpdateContacts = GetNamedAction(nameof(ContactsRedactionController.GetContactsRedaction), "Upravit kontakty", "updateContacts"); //TODO ADD FORM

			responseModel.AddAction(GetQueriedAppAction(nameof(TranslationsController.GetTranslations), "translations_change",
				new Dictionary<string, string> {
					{ "size", "20" },
					{ "offset", "0" } }));

			return Ok(responseModel);
		}

		private NamedAction GetAddArticleAction(string baseUrl, string sectionTranslation, int pageNum, string countryCode)
				=> GetNamedAction(nameof(ArticlesController.AddArticle), $"Přidat článek do {sectionTranslation}", "add_article",
						ArticleFormFactory.GetAddArticleForm("", $"page{pageNum}_title_{{id}}", "", $"page{pageNum}_text_{{id}}", pageNum));
	}
}
