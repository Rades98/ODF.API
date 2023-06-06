﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using ODF.API.Controllers.Base;
using ODF.API.FormFactories;
using ODF.API.Registration.SettingModels;
using ODF.API.RequestModels.Forms.Contacts;
using ODF.API.ResponseModels.Contacts.Create;
using ODF.API.ResponseModels.Contacts.Delete;
using ODF.API.ResponseModels.Contacts.Update;
using ODF.API.ResponseModels.Exceptions;
using ODF.API.Responses;
using ODF.AppLayer.Consts;
using ODF.AppLayer.CQRS.Contact.Commands;
using ODF.AppLayer.Services.Interfaces;

namespace ODF.API.Controllers.Contacts
{
	public class ContactPersonsController : BaseController
	{
		public ContactPersonsController(IMediator mediator, IOptions<ApiSettings> apiSettings, IActionDescriptorCollectionProvider adcp, ITranslationsProvider translationsProvider) : base(mediator, apiSettings, adcp, translationsProvider)
		{
		}

		[HttpPost(Name = nameof(UpdateContactPerson))]
		[Authorize(Roles = UserRoles.Admin)]
		[ProducesResponseType(typeof(UpdateContactPersonResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> UpdateContactPerson([FromRoute] string countryCode, [FromBody] UpdateContactPersonForm form, CancellationToken cancellationToken)
		{
			if (await Mediator.Send(new UpdateContactPersonCommand(form.Email, form.Title, form.Name, form.Surname, form.Roles, form.Base64Image, form.Id, form.Order), cancellationToken))
			{
				return Ok(new UpdateContactPersonResponseModel());
			}

			return CustomApiResponses.InternalServerError(new ExceptionResponseModel("Vyskytla se chyba při aktualizaci kontaktní osoby")); // TODO add validation with form
		}

		[HttpPut(Name = nameof(AddContactPerson))]
		[Authorize(Roles = UserRoles.Admin)]
		[ProducesResponseType(typeof(CreateContactPersonResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(CreateContactPersonResponseModel), StatusCodes.Status422UnprocessableEntity)]
		[ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> AddContactPerson([FromRoute] string countryCode, [FromBody] AddContactPersonForm form, CancellationToken cancellationToken)
		{
			var validationResponse = await Mediator.Send(new AddContactPersonCommand(form.Email, form.Title, form.Name, form.Surname, form.Roles, form.Base64Image), cancellationToken);

			if (validationResponse.IsOk)
			{
				return Ok(new CreateContactPersonResponseModel());
			}

			if (validationResponse.Errors.Any())
			{
				var responseForm = ContactFormFactory.GetAddContactPersonForm(validationResponse.Errors, form);
				return UnprocessableEntity(new CreateContactPersonResponseModel(responseForm));
			}

			return CustomApiResponses.InternalServerError(new ExceptionResponseModel("Vyskytla se chyba při tvorbě kontaktní osoby"));
		}

		[HttpDelete(Name = nameof(RemoveContactPerson))]
		[Authorize(Roles = UserRoles.Admin)]
		[ProducesResponseType(typeof(DeleteContactPersonResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> RemoveContactPerson([FromRoute] string countryCode, [FromBody] RemoveContactPersonForm form, CancellationToken cancellationToken)
		{
			if (await Mediator.Send(new RemoveContactPersonCommand(form.Id), cancellationToken))
			{
				return Ok(new DeleteContactPersonResponseModel());
			}

			return CustomApiResponses.InternalServerError(new ExceptionResponseModel("Vyskytla se chyba při mazání kontaktní osoby")); // TODO add validation with form
		}
	}
}
