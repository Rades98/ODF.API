﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using ODF.API.Attributes.HtttpMethodAttributes;
using ODF.API.Controllers.Base;
using ODF.API.Controllers.Contacts;
using ODF.API.Controllers.Lineup;
using ODF.API.FormComposers;
using ODF.API.ResponseModels.Common;
using ODF.API.ResponseModels.Exceptions;
using ODF.API.ResponseModels.Redaction;
using ODF.AppLayer.Extensions;
using ODF.AppLayer.Services.Interfaces;
using ODF.Domain.Constants;
using ODF.Domain.SettingModels;

namespace ODF.API.Controllers
{
	[Authorize(Roles = UserRoles.Admin)]
	[CountryCodeFilter("cz")]
	public class RedactionController : BaseController
	{
		public RedactionController(IMediator mediator, IOptions<ApiSettings> apiSettings, IActionDescriptorCollectionProvider adcp, ITranslationsProvider translationsProvider)
			: base(mediator, apiSettings, adcp, translationsProvider)
		{
		}

		[HttpGet(Name = nameof(GetRedaction))]
		[ProducesResponseType(typeof(RedactionResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(typeof(UnauthorizedExceptionResponseModel), StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> GetRedaction(CancellationToken cancellationToken)
		{
			var transaltions = await TranslationsProvider.GetTranslationsAsync(CountryCode, cancellationToken);

			var responseModel = new RedactionResponseModel("Redakce");

			responseModel.AddAboutArticle = GetAddArticleAction(transaltions.Get("menu_about"), 0);

			responseModel.AddAssociationArticle = GetAddArticleAction(transaltions.Get("menu_association"), 1);

			responseModel.UpdateLineup = GetNamedAction(nameof(LineupRedactionController.GetLineupRedaction), $"Upavit lineup", "update_lineup");

			responseModel.UpdateContacts = GetNamedAction(nameof(ContactRedactionController.GetContactsRedaction), "Upravit kontakty", "update_contacts");

			responseModel.AddAction(GetQueriedAppAction(nameof(TranslationsController.GetTranslations), "translations_change",
				new Dictionary<string, string> {
					{ "size", "20" },
					{ "offset", "0" } }));

			return Ok(responseModel);
		}

		private NamedAction GetAddArticleAction(string sectionTranslation, int pageNum)
				=> GetNamedAction(nameof(ArticleController.AddArticle), $"Přidat článek do {sectionTranslation}", "add_article",
						ArticleFormComposer.GetAddArticleForm(new()
						{
							Title = "",
							PageId = pageNum,
							Text = "",
							TextTranslationCode = $"page{pageNum}text{{id}}",
							TitleTranslationCode = $"page{pageNum}title{{id}}"
						}));
	}
}
