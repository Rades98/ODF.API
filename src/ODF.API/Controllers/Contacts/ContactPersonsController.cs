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
using ODF.API.ResponseModels.Contacts.Update;
using ODF.API.ResponseModels.Exceptions;
using ODF.AppLayer.CQRS.Contact.Commands;
using ODF.AppLayer.Services.Interfaces;
using ODF.Domain.Constants;
using ODF.Domain.SettingModels;

namespace ODF.API.Controllers.Contacts
{
	public class ContactPersonsController : BaseController
	{
		public ContactPersonsController(IMediator mediator, IOptions<ApiSettings> apiSettings, IActionDescriptorCollectionProvider adcp, ITranslationsProvider translationsProvider)
			: base(mediator, apiSettings, adcp, translationsProvider)
		{
		}

		[HttpPut(Name = nameof(UpdateContactPerson))]
		[Authorize(Roles = UserRoles.Admin)]
		[CountryCodeFilter("cz")]
		[ProducesResponseType(typeof(UpdateContactPersonResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> UpdateContactPerson([FromBody] UpdateContactPersonForm form, CancellationToken cancellationToken)
		{
			var validationResult = await Mediator.Send(new UpdateContactPersonCommand(form), cancellationToken);
			if (validationResult.IsOk)
			{
				return Ok(new UpdateContactPersonResponseModel());
			}

			if (validationResult.Errors.Any())
			{
				var responseForm = ContactFormComposer.GetUpdateContactPersonForm(form, validationResult.Errors);
				return UnprocessableEntity(new CreateContactPersonResponseModel(responseForm));
			}

			return InternalServerError(new ExceptionResponseModel("Vyskytla se chyba při aktualizaci kontaktní osoby"));
		}

		[HttpPost(Name = nameof(AddContactPerson))]
		[Authorize(Roles = UserRoles.Admin)]
		[CountryCodeFilter("cz")]
		[ProducesResponseType(typeof(CreateContactPersonResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(CreateContactPersonResponseModel), StatusCodes.Status422UnprocessableEntity)]
		[ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> AddContactPerson([FromBody] AddContactPersonForm form, CancellationToken cancellationToken)
		{
			var validationResult = await Mediator.Send(new AddContactPersonCommand(form), cancellationToken);

			if (validationResult.IsOk)
			{
				return Ok(new CreateContactPersonResponseModel());
			}

			if (validationResult.Errors.Any())
			{
				var responseForm = ContactFormComposer.GetAddContactPersonForm(form, validationResult.Errors);
				return UnprocessableEntity(new CreateContactPersonResponseModel(responseForm));
			}

			return InternalServerError(new ExceptionResponseModel("Vyskytla se chyba při tvorbě kontaktní osoby"));
		}

		[HttpDelete(Name = nameof(RemoveContactPerson))]
		[Authorize(Roles = UserRoles.Admin)]
		[CountryCodeFilter("cz")]
		[ProducesResponseType(typeof(DeleteContactPersonResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> RemoveContactPerson([FromBody] RemoveContactPersonForm form, CancellationToken cancellationToken)
		{
			var validationResult = await Mediator.Send(new RemoveContactPersonCommand(form), cancellationToken);

			if (validationResult.IsOk)
			{
				return Ok(new DeleteContactPersonResponseModel());
			}

			if (validationResult.Errors.Any())
			{
				var responseForm = ContactFormComposer.GetRemoveContactPersonForm(form, validationResult.Errors);
				return UnprocessableEntity(new CreateContactPersonResponseModel(responseForm));
			}

			return InternalServerError(new ExceptionResponseModel("Vyskytla se chyba při mazání kontaktní osoby"));
		}
	}
}
