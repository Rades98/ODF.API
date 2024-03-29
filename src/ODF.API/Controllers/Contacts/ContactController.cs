﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using ODF.API.Attributes.HtttpMethodAttributes;
using ODF.API.Controllers.Base;
using ODF.API.Extensions.MappingExtensions;
using ODF.API.FormComposers;
using ODF.API.RequestModels.Forms.Contacts;
using ODF.API.ResponseModels.Contacts.GetContacts;
using ODF.API.ResponseModels.Contacts.Update;
using ODF.API.ResponseModels.Exceptions;
using ODF.AppLayer.CQRS.Contact.Commands;
using ODF.AppLayer.CQRS.Contact.Queries;
using ODF.AppLayer.Services.Interfaces;
using ODF.Domain.Constants;
using ODF.Domain.SettingModels;

namespace ODF.API.Controllers.Contacts
{
	public class ContactController : BaseController
	{
		public ContactController(IMediator mediator, IOptions<ApiSettings> apiSettings, IActionDescriptorCollectionProvider adcp, ITranslationsProvider translationsProvider)
			: base(mediator, apiSettings, adcp, translationsProvider)
		{
			ApiSettings = apiSettings;
		}

		public IOptions<ApiSettings> ApiSettings { get; }

		[HttpGet(Name = nameof(GetContacts))]
		[ProducesResponseType(typeof(ContactResponseModel), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetContacts()
		{
			var contact = await Mediator.Send(new GetContactQuery(CountryCode));

			return Ok(ContactsResponseMapper.GetContactResponse(contact));
		}

		[HttpPut(Name = nameof(UpdateContact))]
		[Authorize(Roles = UserRoles.Admin)]
		[CountryCodeFilter("cz")]
		[ProducesResponseType(typeof(UpdateContactResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> UpdateContact([FromBody] UpdateContactForm form, CancellationToken cancellationToken)
		{
			var validationResult = await Mediator.Send(new UpdateContactCommand(form), cancellationToken);
			if (validationResult.IsOk)
			{
				return Ok(new UpdateContactResponseModel());
			}

			if (validationResult.Errors.Any())
			{
				var resultForm = ContactFormComposer.GetUpdateContactForm(form, validationResult.Errors);
				return UnprocessableEntity(new UpdateContactResponseModel(resultForm));
			}

			return InternalServerError(new ExceptionResponseModel("Vyskytla se chyba při aktualizaci kontaktu"));
		}
	}
}
