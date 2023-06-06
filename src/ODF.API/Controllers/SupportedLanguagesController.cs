using System.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using ODF.API.Controllers.Base;
using ODF.API.Registration.SettingModels;
using ODF.API.ResponseModels.Exceptions;
using ODF.API.ResponseModels.LanguageMutations;
using ODF.AppLayer.CQRS.Translations.Queries;
using ODF.Enums;

namespace ODF.API.Controllers
{
	public class SupportedLanguagesController : BaseController
	{
		public SupportedLanguagesController(IMediator mediator, IOptions<ApiSettings> apiSettings, IActionDescriptorCollectionProvider adcp) : base(mediator, apiSettings, adcp)
		{
		}

		[HttpGet(Name = nameof(GetSupportedLanguages))]
		[ProducesResponseType(typeof(LanguageResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetSupportedLanguages([FromRoute] string countryCode, CancellationToken cancellationToken)
		{
			string actionParttext = await Mediator.Send(new GetTranslationQuery("Přepnout do {0}", "app_language_switch", countryCode), cancellationToken);
			var languages = Languages.GetAll().Select(lang =>
			{
				string actionName = string.Format(actionParttext, lang.GetCountryCode());
				var languageModel = new LanguageModel(lang.Name, lang.GetCountryCode());

				if (lang.GetCountryCode().ToLower() != countryCode.ToLower())
				{
					languageModel.ChangeLanguage = GetNamedAction(nameof(NavigationController.GetNavigation), actionName, "nav");
				}

				return languageModel;
			});

			string title = await Mediator.Send(new GetTranslationQuery("Jazyk", "app_language", countryCode), cancellationToken);
			var responseModel = new LanguageResponseModel(languages, title);

			return Ok(responseModel);
		}
	}
}
