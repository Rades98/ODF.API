using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ODF.API.Controllers.Base;
using ODF.API.FormFactories;
using ODF.API.Registration.SettingModels;
using ODF.API.RequestModels.Forms;
using ODF.API.ResponseModels.Common;
using ODF.API.ResponseModels.Exceptions;
using ODF.API.ResponseModels.LanguageMutations;
using ODF.API.Responses;
using ODF.AppLayer.Consts;
using ODF.AppLayer.CQRS.Translations.Commands;
using ODF.AppLayer.CQRS.Translations.Queries;
using ODF.Enums;

namespace ODF.API.Controllers
{
	public class TranslationsController : BaseController
	{
		public TranslationsController(IMediator mediator, IOptions<ApiSettings> apiSettings) : base(mediator, apiSettings)
		{
		}

		[HttpGet(Name = nameof(GetTranslations))]
		[Authorize(Roles = UserRoles.Admin)]
		[ProducesResponseType(typeof(GetTranslationsResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(UnauthorizedExceptionResponseModel), StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> GetTranslations([FromRoute] string countryCode, int size, int offset, CancellationToken cancellationToken)
		{
			if (countryCode.ToUpper() != Languages.Czech.GetCountryCode())
			{
				return UnprocessableEntity("This action is supported for CZ language only");
			}

			var translations = await Mediator.Send(new GetTranslationsQuery(countryCode, size, offset), cancellationToken);
			var deTranslations = await Mediator.Send(new GetTranslationsQuery(Languages.Deutsch.GetCountryCode(), size, offset), cancellationToken);
			var enTranslations = await Mediator.Send(new GetTranslationsQuery(Languages.English.GetCountryCode(), size, offset), cancellationToken);

			var responseModel = new GetTranslationsResponseModel(ApiSettings.ApiUrl, countryCode, "Správa překladů", $"/translations?size={size}&offset={offset}");
			responseModel.Translations = translations.Translations.Select(tr =>
			{
				var model = new GetTranslationResponseModel(ApiSettings.ApiUrl, countryCode, tr.TranslationCode, tr.Text);

				model.ChangeEnTranslation = new NamedAction($"{ApiSettings.ApiUrl}/{countryCode}/translations", "změnit AJ překlad", "transalation_en_submit", HttpMethods.Post,
					TranslationFormFactory.GetChangeTranslationForm(model.TranslationCode, enTranslations.Translations.FirstOrDefault(tr => tr.TranslationCode == model.TranslationCode)?.Text ?? "", Languages.English.GetCountryCode()));

				model.ChangeDeTranslation = new NamedAction($"{ApiSettings.ApiUrl}/{countryCode}/translations", "změnit DE překlad", "transalation_de_submit", HttpMethods.Post,
					TranslationFormFactory.GetChangeTranslationForm(model.TranslationCode, deTranslations.Translations.FirstOrDefault(tr => tr.TranslationCode == model.TranslationCode)?.Text ?? "", Languages.Deutsch.GetCountryCode()));

				model.ChangeTranslation = new NamedAction($"{ApiSettings.ApiUrl}/{countryCode}/translations", "změnit CZ překlad", "transalation_cz_submit", HttpMethods.Post,
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

		[HttpPost(Name = nameof(ChangeTranslation))]
		[Authorize(Roles = UserRoles.Admin)]
		[ProducesResponseType(typeof(UpdateTranslationResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(typeof(BadRequestExceptionResponseModel), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(UnauthorizedExceptionResponseModel), StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> ChangeTranslation(string countryCode, [FromBody] ChangeTranslationForm form, CancellationToken cancellationToken)
		{
			if (countryCode.ToUpper() != Languages.Czech.GetCountryCode())
			{
				var navAction = new NamedAction(ApiSettings.ApiUrl + "/cz/navigation", "Přepnout do CZ", "nav", HttpMethods.Get);
				return BadRequest(new BadRequestExceptionResponseModel("This action is supported for CZ language only", "Switch to cz", altAction: navAction));
			}

			bool result = await Mediator.Send(new ModifyTransaltionCommand(form.CountryCode, form.TranslationCode, form.Text), cancellationToken);

			if (result)
			{
				var responseModel = new UpdateTranslationResponseModel(ApiSettings.ApiUrl, countryCode,
					TranslationFormFactory.GetChangeTranslationForm(form.TranslationCode, form.Text, Languages.Deutsch.GetCountryCode()),
					$"Proměnná {form.TranslationCode} byla úspěšně přeložena pro {form.CountryCode}: {form.Text}.");

				return Ok(responseModel);
			}

			return CustomApiResponses.InternalServerError(new ExceptionResponseModel($"Při překladu {form.TranslationCode} pro {form.CountryCode} na hodnotu {form.Text} došlo k chybě."));
		}
	}
}
