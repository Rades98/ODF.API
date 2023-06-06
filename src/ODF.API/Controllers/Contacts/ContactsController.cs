﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using ODF.API.Controllers.Base;
using ODF.API.Registration.SettingModels;
using ODF.API.RequestModels.Forms.Contacts;
using ODF.API.ResponseComposers.Contacts;
using ODF.API.ResponseModels.Contacts.GetContacts;
using ODF.API.ResponseModels.Contacts.Update;
using ODF.API.ResponseModels.Exceptions;
using ODF.API.Responses;
using ODF.AppLayer.Consts;
using ODF.AppLayer.CQRS.Contact.Commands;
using ODF.AppLayer.CQRS.Contact.Queries;
using ODF.AppLayer.Services.Interfaces;

namespace ODF.API.Controllers.Contacts
{
	public class ContactsController : BaseController
	{
		public ContactsController(IMediator mediator, IOptions<ApiSettings> apiSettings, IActionDescriptorCollectionProvider adcp, ITranslationsProvider translationsProvider) : base(mediator, apiSettings, adcp, translationsProvider)
		{
		}

		[HttpGet(Name = nameof(GetContacts))]
		[ProducesResponseType(typeof(ContactResponseModel), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetContacts([FromRoute] string countryCode)
		{
			var contact = await Mediator.Send(new GetContactQuery(countryCode));

			return Ok(ContactsResponseComposer.GetContactResponse(countryCode, ApiBaseUrl, contact));
		}

		[HttpPost(Name = nameof(UpdateContact))]
		[Authorize(Roles = UserRoles.Admin)]
		[ProducesResponseType(typeof(UpdateContactResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> UpdateContact([FromRoute] string countryCode, [FromBody] UpdateContactForm form)
		{
			if (await Mediator.Send(new UpdateContactCommand(form.EventName, form.EventManager, form.Email)))
			{
				return Ok(new UpdateContactResponseModel());
			}

			return CustomApiResponses.InternalServerError(new ExceptionResponseModel("Vyskytla se chyba při aktualizaci kontaktu"));  // TODO add validation with form
		}
	}
}
