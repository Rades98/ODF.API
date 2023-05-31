using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Options;
using ODF.API.FormFactories;
using ODF.API.Registration.SettingModels;
using ODF.API.RequestModels.Forms;
using ODF.API.ResponseModels.About;
using ODF.API.ResponseModels.Common;
using ODF.API.ResponseModels.Exceptions;
using ODF.API.ResponseModels.LanguageMutations;
using ODF.API.Responses;
using ODF.AppLayer.Consts;
using ODF.AppLayer.CQRS.Translations.Commands;
using ODF.AppLayer.CQRS.Translations.Queries;
using ODF.Enums;
using System.Data;

namespace ODF.API.Controllers
{
    public class LanguagesController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ApiSettings _settings;

        public LanguagesController(IMediator mediator, IOptions<ApiSettings> apiSettings)
        {
            _mediator = mediator;
            _settings = apiSettings.Value;
        }


        [HttpGet("/{countryCode}/supportedLanguages")]
        [ProducesResponseType(typeof(LanguageResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSupportedLanguages([FromRoute] string countryCode, CancellationToken cancellationToken)
        {
            var actionParttext = await _mediator.Send(new GetTranslationQuery("Přepnout do {0}", "app_language_switch", countryCode), cancellationToken);
            var languages = Languages.GetAll().Select(lang =>
            {
                var actionName = string.Format(actionParttext, lang.GetCountryCode());
                var languageModel = new LanguageModel(lang.Name, lang.GetCountryCode());

                if (lang.GetCountryCode().ToLower() != countryCode.ToLower())
                {
                    languageModel.ChangeLanguage = new(_settings.ApiUrl + $"/{lang.GetCountryCode()}/navigation", actionName, "nav", HttpMethods.Get);
                }

                return languageModel;
            });

            var title = await _mediator.Send(new GetTranslationQuery("Jazyk", "app_language", countryCode), cancellationToken);
            var responseModel = new LanguageResponseModel(_settings.ApiUrl, languages, title, countryCode);

            return Ok(responseModel);
        }

        /* 2RADEK: Tejto metode nerozumiem. K comu bude vlastne sluzit? Napevno sa tu riesia tri jazyky. */
        [HttpGet("{countryCode}/translations")]
        [Authorize(Roles = UserRoles.Admin)]
        [ProducesResponseType(typeof(GetTranslationsResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UnauthorizedExceptionResponseModel), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetTranslations([FromRoute] string countryCode, int size, int offset, CancellationToken cancellationToken)
        {
            if (countryCode.ToUpper() != Languages.Czech.GetCountryCode())
            {
                return UnprocessableEntity("This action is supported for CZ language only");
            }

            var translations = await _mediator.Send(new GetTranslationsQuery(countryCode, size, offset), cancellationToken);
            var deTranslations = await _mediator.Send(new GetTranslationsQuery(Languages.Deutsch.GetCountryCode(), size, offset), cancellationToken);
            var enTranslations = await _mediator.Send(new GetTranslationsQuery(Languages.English.GetCountryCode(), size, offset), cancellationToken);

            var responseModel = new GetTranslationsResponseModel(_settings.ApiUrl, countryCode, "Správa překladů", $"/translations?size={size}&offset={offset}");
            responseModel.Translations = translations.Translations.Select(tr =>
            {
                var model = new GetTranslationResponseModel(_settings.ApiUrl, countryCode, tr.TranslationCode, tr.Text);

                model.ChangeEnTranslation = new NamedAction($"{_settings.ApiUrl}/{countryCode}/translations", "změnit AJ překlad", "transalation_en_submit", HttpMethods.Post,
                    TranslationFormFactory.GetChangeTranslationForm(model.TranslationCode, enTranslations.Translations.FirstOrDefault(tr => tr.TranslationCode == model.TranslationCode)?.Text ?? "", Languages.English.GetCountryCode()));

                model.ChangeDeTranslation = new NamedAction($"{_settings.ApiUrl}/{countryCode}/translations", "změnit DE překlad", "transalation_de_submit", HttpMethods.Post,
                    TranslationFormFactory.GetChangeTranslationForm(model.TranslationCode, deTranslations.Translations.FirstOrDefault(tr => tr.TranslationCode == model.TranslationCode)?.Text ?? "", Languages.Deutsch.GetCountryCode()));

                model.ChangeTranslation = new NamedAction($"{_settings.ApiUrl}/{countryCode}/translations", "změnit CZ překlad", "transalation_cz_submit", HttpMethods.Post,
                    TranslationFormFactory.GetChangeTranslationForm(model.TranslationCode, model.Text, Languages.Deutsch.GetCountryCode()));

                return model;
            });

            if (offset > 0)
            {
                responseModel.AddAction($"/{countryCode}/translations?size={size}&offset={offset - 1}", "translations_previous", HttpMethods.Get);
            }

            if (translations.Count > offset * size + size)
            {
                responseModel.AddAction($"/{countryCode}/translations?size={size}&offset={offset + 1}", "translations_next", HttpMethods.Get);
            }

            return Ok(responseModel);
        }

        [HttpPost("{countryCode}/translations")]
        [Authorize(Roles = UserRoles.Admin)]
        [ProducesResponseType(typeof(UpdateTranslationResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(BadRequestExceptionResponseModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedExceptionResponseModel), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> PostTranslations(string countryCode, [FromBody] ChangeTranslationForm form, CancellationToken cancellationToken)
        {
            if (countryCode.ToUpper() != Languages.Czech.GetCountryCode())
            {
                var navAction = new NamedAction(_settings.ApiUrl + "/cz/navigation", "Přepnout do CZ", "nav", HttpMethods.Get);
                return BadRequest(new BadRequestExceptionResponseModel("This action is supported for CZ language only", "Switch to cz", altAction: navAction));
            }

            var result = await _mediator.Send(new ModifyTransaltionCommand(form.CountryCode, form.TranslationCode, form.Text), cancellationToken);

            if (result)
            {
                var responseModel = new UpdateTranslationResponseModel(_settings.ApiUrl, countryCode,
                    TranslationFormFactory.GetChangeTranslationForm(form.TranslationCode, form.Text, Languages.Deutsch.GetCountryCode()),
                    $"Proměnná {form.TranslationCode} byla úspěšně přeložena pro {form.CountryCode}: {form.Text}.");

                return Ok(responseModel);
            }

            return (IActionResult)CustomApiResponses.InternalServerError(new ExceptionResponseModel($"Při překladu {form.TranslationCode} pro {form.CountryCode} na hodnotu {form.Text} došlo k chybě."));
        }
    }
}
