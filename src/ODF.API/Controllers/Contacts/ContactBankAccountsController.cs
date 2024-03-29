﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using ODF.API.Attributes.HtttpMethodAttributes;
using ODF.API.Controllers.Base;
using ODF.API.FormComposers;
using ODF.API.RequestModels.Forms.Contacts;
using ODF.API.ResponseModels.Contacts.Create;
using ODF.API.ResponseModels.Contacts.Delete;
using ODF.API.ResponseModels.Exceptions;
using ODF.AppLayer.CQRS.Contact.Commands;
using ODF.AppLayer.Services.Interfaces;
using ODF.Domain.Constants;
using ODF.Domain.SettingModels;

namespace ODF.API.Controllers.Contacts
{
	public class ContactBankAccountsController : BaseController
	{
		public ContactBankAccountsController(IMediator mediator, IOptions<ApiSettings> apiSettings, IActionDescriptorCollectionProvider adcp, ITranslationsProvider translationsProvider)
			: base(mediator, apiSettings, adcp, translationsProvider)
		{
		}

		[HttpPost(Name = nameof(AddBankAccount))]
		[Authorize(Roles = UserRoles.Admin)]
		[CountryCodeFilter("cz")]
		[ProducesResponseType(typeof(CreateContactBankAccResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(CreateContactBankAccResponseModel), StatusCodes.Status422UnprocessableEntity)]
		[ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> AddBankAccount([FromBody] AddBankAccountForm form, CancellationToken cancellationToken)
		{
			var validationResult = await Mediator.Send(new AddBankAccountCommand(form), cancellationToken);

			if (validationResult.IsOk)
			{
				return Ok(new CreateContactBankAccResponseModel());
			}

			if (validationResult.Errors.Any())
			{
				var responseForm = ContactFormComposer.GetAddBankAcountForm(form, validationResult.Errors);
				return UnprocessableEntity(new CreateContactBankAccResponseModel(responseForm));
			}

			return InternalServerError(new ExceptionResponseModel("Vyskytla se chyba při přidávání bankovního účtu"));
		}

		[HttpDelete(Name = nameof(RemoveBankAccount))]
		[Authorize(Roles = UserRoles.Admin)]
		[CountryCodeFilter("cz")]
		[ProducesResponseType(typeof(DeleteContactBankAccResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> RemoveBankAccount([FromBody] RemoveBankAccountForm form, CancellationToken cancellationToken)
		{
			var validationResult = await Mediator.Send(new RemoveBankAccountCommand(form), cancellationToken);

			if (validationResult.IsOk)
			{
				return Ok(new DeleteContactBankAccResponseModel());
			}

			if (validationResult.Errors.Any())
			{
				var responseForm = ContactFormComposer.GetRemoveBankAcountForm(form, validationResult.Errors);
				return UnprocessableEntity(new DeleteContactBankAccResponseModel(responseForm));
			}

			return InternalServerError(new ExceptionResponseModel("Vyskytla se chyba při mazání bankovního účtu"));
		}
	}
}
