using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using ODF.API.Attributes.HtttpMethodAttributes;
using ODF.API.Controllers.Base;
using ODF.API.Controllers.Contacts;
using ODF.API.Controllers.Lineup;
using ODF.API.FormFactories;
using ODF.API.ResponseModels.Common;
using ODF.API.ResponseModels.Exceptions;
using ODF.API.ResponseModels.Redaction;
using ODF.AppLayer.Extensions;
using ODF.AppLayer.Services.Interfaces;
using ODF.Domain.Constants;
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

			responseModel.AddAboutArticle = GetAddArticleAction(transaltions.Get("menu_about"), 0);

			responseModel.AddAssociationArticle = GetAddArticleAction(transaltions.Get("menu_association"), 1);

			responseModel.UpdateLineup = GetNamedAction(nameof(LineupRedactionController.GetLineupRedaction), $"Upavit lineup", "update_lineup");

			responseModel.UpdateContacts = GetNamedAction(nameof(ContactsRedactionController.GetContactsRedaction), "Upravit kontakty", "update_contacts");

			responseModel.AddAction(GetQueriedAppAction(nameof(TranslationsController.GetTranslations), "translations_change",
				new Dictionary<string, string> {
					{ "size", "20" },
					{ "offset", "0" } }));

			return Ok(responseModel);
		}

		private NamedAction GetAddArticleAction(string sectionTranslation, int pageNum)
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
