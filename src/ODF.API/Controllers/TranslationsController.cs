using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using ODF.API.Attributes.HtttpMethodAttributes;
using ODF.API.Controllers.Base;
using ODF.API.FormFactories;
using ODF.API.RequestModels.Forms;
using ODF.API.ResponseModels.Exceptions;
using ODF.API.ResponseModels.LanguageMutations;
using ODF.AppLayer.CQRS.Translations.Commands;
using ODF.AppLayer.CQRS.Translations.Queries;
using ODF.AppLayer.Services.Interfaces;
using ODF.Domain;
using ODF.Domain.Constants;
using ODF.Domain.SettingModels;

namespace ODF.API.Controllers
{
	public class TranslationsController : BaseController
	{
		public TranslationsController(IMediator mediator, IOptions<ApiSettings> apiSettings, IActionDescriptorCollectionProvider adcp, ITranslationsProvider translationsProvider)
			: base(mediator, apiSettings, adcp, translationsProvider)
		{
		}

		[HttpGet(Name = nameof(GetTranslations))]
		[Authorize(Roles = UserRoles.Admin)]
		[CountryCodeFilter("cz")]
		[ProducesResponseType(typeof(GetTranslationsResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(UnauthorizedExceptionResponseModel), StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> GetTranslations([FromRoute] string countryCode, int size, int offset, CancellationToken cancellationToken)
		{
			var translations = await Mediator.Send(new GetTranslationsQuery(countryCode, size, offset), cancellationToken);

			var responseModel = new GetTranslationsResponseModel("Správa překladů");
			responseModel.Translations = translations.Translations.Select(tr =>
			{
				var model = new GetTranslationResponseModel(tr.TranslationCode, tr.Text);

				model.ChangeTranslation = GetNamedAction(nameof(ChangeTranslation), "změnit AJ překlad", "transalation_en_submit",
					TranslationFormFactory.GetChangeTranslationForm(new() { CountryCode = Languages.English.GetCountryCode(), TranslationCode = model.TranslationCode }));

				model.ChangeTranslation = GetNamedAction(nameof(ChangeTranslation), "změnit DE překlad", "transalation_de_submit",
					TranslationFormFactory.GetChangeTranslationForm(new() { CountryCode = Languages.Deutsch.GetCountryCode(), TranslationCode = model.TranslationCode }));

				model.ChangeTranslation = GetNamedAction(nameof(ChangeTranslation), "změnit CZ překlad", "transalation_cz_submit",
					TranslationFormFactory.GetChangeTranslationForm(new() { CountryCode = Languages.Czech.GetCountryCode(), TranslationCode = model.TranslationCode }));

				return model;
			});

			if (offset > 0)
			{
				responseModel.AddAction(GetQueriedAppAction(nameof(GetTranslations), "translations_previous", new Dictionary<string, string> {
					{ nameof(size), $"{size}" },
					{ nameof(offset), $"{offset - 1}" } }));
			}

			if (translations.Count > offset * size + size)
			{
				responseModel.AddAction(GetQueriedAppAction(nameof(GetTranslations), "translations_next", new Dictionary<string, string> {
					{ nameof(size), $"{size}" },
					{ nameof(offset), $"{offset + 1}" } }));
			}

			return Ok(responseModel);
		}

		[HttpPost(Name = nameof(ChangeTranslation))]
		[Authorize(Roles = UserRoles.Admin)]
		[CountryCodeFilter("cz")]
		[ProducesResponseType(typeof(UpdateTranslationResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(typeof(BadRequestExceptionResponseModel), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(UnauthorizedExceptionResponseModel), StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> ChangeTranslation(string countryCode, [FromBody] ChangeTranslationForm form, CancellationToken cancellationToken)
		{
			bool result = await Mediator.Send(new ModifyTransaltionCommand(form.CountryCode, form.TranslationCode, form.Text), cancellationToken);

			if (result)
			{
				var responseModel = new UpdateTranslationResponseModel(
					TranslationFormFactory.GetChangeTranslationForm(new() { CountryCode = countryCode, TranslationCode = form.TranslationCode, Text = form.Text }),
					$"Proměnná {form.TranslationCode} byla úspěšně přeložena pro {form.CountryCode}: {form.Text}.");

				return Ok(responseModel);
			}

			return InternalServerError(new ExceptionResponseModel($"Při překladu {form.TranslationCode} pro {form.CountryCode} na hodnotu {form.Text} došlo k chybě."));
		}
	}
}
