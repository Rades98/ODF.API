using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using ODF.API.Attributes.HtttpMethodAttributes;
using ODF.API.Controllers.Base;
using ODF.API.FormComposers;
using ODF.API.RequestModels.Forms.Contacts;
using ODF.API.ResponseModels.Contacts.Update;
using ODF.API.ResponseModels.Exceptions;
using ODF.AppLayer.CQRS.Contact.Commands;
using ODF.AppLayer.Services.Interfaces;
using ODF.Domain.Constants;
using ODF.Domain.SettingModels;

namespace ODF.API.Controllers.Contacts
{
	public class ContactAddressController : BaseController
	{
		public ContactAddressController(IMediator mediator, IOptions<ApiSettings> apiSettings, IActionDescriptorCollectionProvider adcp, ITranslationsProvider translationsProvider)
			: base(mediator, apiSettings, adcp, translationsProvider)
		{
		}

		[HttpPost(Name = nameof(UpdateAddress))]
		[Authorize(Roles = UserRoles.Admin)]
		[CountryCodeFilter("cz")]
		[ProducesResponseType(typeof(UpdateContactAddressResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> UpdateAddress([FromBody] UpdateAddressForm form)
		{
			var validationResult = await Mediator.Send(new UpdateContactAddressCommand(form.Street, form.City, form.PostalCode, form.Country));

			if (validationResult.IsOk)
			{
				return Ok(new UpdateContactAddressResponseModel());
			}

			if (validationResult.Errors.Any())
			{
				return UnprocessableEntity(new UpdateContactAddressResponseModel(ContactFormComposer.GetUpdateAddressForm(form)));
			}

			return InternalServerError(new ExceptionResponseModel("Vyskytla se chyba při aktualizaci adresy"));
		}
	}
}
