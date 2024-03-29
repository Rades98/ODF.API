﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using ODF.API.Attributes.HtttpMethodAttributes;
using ODF.API.Controllers.Base;
using ODF.API.FormComposers;
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
	[Authorize(Roles = UserRoles.Admin)]
	[CountryCodeFilter("cz")]
	public class TranslationsController : BaseController
	{
		public TranslationsController(IMediator mediator, IOptions<ApiSettings> apiSettings, IActionDescriptorCollectionProvider adcp, ITranslationsProvider translationsProvider)
			: base(mediator, apiSettings, adcp, translationsProvider)
		{
		}

		[HttpGet(Name = nameof(GetTranslations))]
		[ProducesResponseType(typeof(GetTranslationsResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(UnauthorizedExceptionResponseModel), StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> GetTranslations(int size, int offset, CancellationToken cancellationToken)
		{
			var translations = await Mediator.Send(new GetTranslationsQuery(CountryCode, size, offset), cancellationToken);

			var responseModel = new GetTranslationsResponseModel("Správa překladů");
			responseModel.Translations = translations.Translations.Select(tr =>
			{
				var model = new GetTranslationResponseModel(tr.TranslationCode, tr.Text);

				model.ChangeTranslation = GetNamedAction(nameof(ChangeTranslation), "změnit AJ překlad", "transalation_en_submit",
					TranslationFormComposer.GetChangeTranslationForm(new() { TranslationCode = model.TranslationCode, CountryCode = Languages.English.GetCountryCode() }));

				model.ChangeDeTranslation = GetNamedAction(nameof(ChangeTranslation), "změnit DE překlad", "transalation_de_submit",
					TranslationFormComposer.GetChangeTranslationForm(new() { TranslationCode = model.TranslationCode, CountryCode = Languages.Deutsch.GetCountryCode() }));

				model.ChangeEnTranslation = GetNamedAction(nameof(ChangeTranslation), "změnit CZ překlad", "transalation_cz_submit",
					TranslationFormComposer.GetChangeTranslationForm(new() { TranslationCode = model.TranslationCode, CountryCode = Languages.Czech.GetCountryCode() }));

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

		[HttpPut(Name = nameof(ChangeTranslation))]
		[ProducesResponseType(typeof(UpdateTranslationResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(typeof(BadRequestExceptionResponseModel), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(UnauthorizedExceptionResponseModel), StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> ChangeTranslation([FromBody] UpdateTranslationForm form, CancellationToken cancellationToken)
		{
			var validationResult = await Mediator.Send(new UpdateTransaltionCommand(form), cancellationToken);

			if (validationResult.IsOk)
			{
				var responseModel = new UpdateTranslationResponseModel(
					TranslationFormComposer.GetChangeTranslationForm(new() { TranslationCode = form.TranslationCode, Text = form.Text }),
					$"Proměnná {form.TranslationCode} byla úspěšně přeložena pro {form.CountryCode}: {form.Text}.");

				return Ok(responseModel);
			}

			if (validationResult.Errors.Any())
			{
				var responseModel = new UpdateTranslationResponseModel(TranslationFormComposer.GetChangeTranslationForm(new() { TranslationCode = form.TranslationCode, Text = form.Text }));
				return UnprocessableEntity(responseModel);
			}

			return InternalServerError(new ExceptionResponseModel($"Při překladu {form.TranslationCode} pro {form.CountryCode} na hodnotu {form.Text} došlo k chybě."));
		}
	}
}
