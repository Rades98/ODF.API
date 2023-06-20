using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using ODF.API.Attributes.HtttpMethodAttributes;
using ODF.API.Controllers.Base;
using ODF.API.Controllers.Contacts;
using ODF.API.FormFactories;
using ODF.API.ResponseModels.Common;
using ODF.API.ResponseModels.Exceptions;
using ODF.API.ResponseModels.Redaction;
using ODF.AppLayer.Consts;
using ODF.AppLayer.Extensions;
using ODF.AppLayer.Services.Interfaces;
using ODF.Domain.SettingModels;

namespace ODF.API.Controllers
{
	public class RedactionController : BaseController
	{
		public RedactionController(IMediator mediator, IOptions<ApiSettings> apiSettings, IActionDescriptorCollectionProvider adcp, ITranslationsProvider translationsProvider)
			: base(mediator, apiSettings, adcp, translationsProvider)
		{
		}

		[HttpGet(Name = nameof(GetRedaction))]
		[Authorize(Roles = UserRoles.Admin)]
		[CountryCodeFilter("cz")]
		[ProducesResponseType(typeof(RedactionResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(typeof(UnauthorizedExceptionResponseModel), StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> GetRedaction([FromRoute] string countryCode, CancellationToken cancellationToken)
		{
			var transaltions = await TranslationsProvider.GetTranslationsAsync(countryCode, cancellationToken);

			var responseModel = new RedactionResponseModel("Redakce");

			responseModel.AddAboutArticle = GetAddArticleAction(transaltions.Get("nav_about"), 0, countryCode);

			responseModel.AddAssociationArticle = GetAddArticleAction(transaltions.Get("nav_association"), 1, countryCode);

			responseModel.AddLineupItem = GetNamedAction(nameof(LineupController.AddItemToLineup), $"Přidat item do programu", "add_lineup_item",
					LineupItemFormFactory.GetAddLineupItemForm("Místo", "Interpret", "Název představení", "popis vystoupení", "{vystoupeni}_desc", DateTime.Now));

			responseModel.UpdateContacts = GetNamedAction(nameof(ContactsRedactionController.GetContactsRedaction), "Upravit kontakty", "updateContacts");

			responseModel.AddAction(GetQueriedAppAction(nameof(TranslationsController.GetTranslations), "translations_change",
				new Dictionary<string, string> {
					{ "size", "20" },
					{ "offset", "0" } }));

			return Ok(responseModel);
		}

		private NamedAction GetAddArticleAction(string sectionTranslation, int pageNum, string countryCode)
				=> GetNamedAction(nameof(ArticlesController.AddArticle), $"Přidat článek do {sectionTranslation}", "add_article",
						ArticleFormFactory.GetAddArticleForm(new()
						{
							Title = "",
							PageId = pageNum,
							Text = "",
							TextTranslationCode = $"page{pageNum}_text_{{id}}",
							TitleTranslationCode = $"page{pageNum}_title_{{id}}"
						}));
	}
}
