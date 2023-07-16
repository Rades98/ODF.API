using System.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using ODF.API.Controllers.Base;
using ODF.API.ResponseModels.Exceptions;
using ODF.API.ResponseModels.LanguageMutations;
using ODF.AppLayer.Extensions;
using ODF.AppLayer.Services.Interfaces;
using ODF.Domain;
using ODF.Domain.SettingModels;

namespace ODF.API.Controllers
{
	public class SupportedLanguagesController : BaseController
	{
		public SupportedLanguagesController(IMediator mediator, IOptions<ApiSettings> apiSettings, IActionDescriptorCollectionProvider adcp, ITranslationsProvider translationsProvider)
			: base(mediator, apiSettings, adcp, translationsProvider)
		{
		}

		[HttpGet(Name = nameof(GetSupportedLanguages))]
		[ProducesResponseType(typeof(LanguageResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetSupportedLanguages(CancellationToken cancellationToken)
		{
			var translations = await TranslationsProvider.GetTranslationsAsync(CountryCode, cancellationToken);

			var languages = Languages.GetAll().Select(lang =>
			{
				string actionName = string.Format(translations.Get("app_language_switch"), lang.GetCountryCode());
				var languageModel = new LanguageModel(lang.Name, lang.GetCountryCode());

				if (Languages.TryParse(CountryCode, out var language) &&
					language is not null &&
					lang.GetCountryCode() != language.GetCountryCode())
				{
					languageModel.ChangeLanguage = GetNamedAction(nameof(NavigationController.GetNavigation), actionName, "nav");
				}

				return languageModel;
			});

			var responseModel = new LanguageResponseModel(languages, translations.Get("app_language"));

			return Ok(responseModel);
		}
	}
}
