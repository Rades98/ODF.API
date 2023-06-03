using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ODF.API.Controllers.Base;
using ODF.API.Registration.SettingModels;
using ODF.API.RequestModels.Forms.Contacts;
using ODF.API.ResponseModels.Contacts.Update;
using ODF.API.ResponseModels.Exceptions;
using ODF.API.Responses;
using ODF.AppLayer.Consts;
using ODF.AppLayer.CQRS.Contact.Commands;

namespace ODF.API.Controllers.Contacts
{
	public class ContactAddressController : BaseController
	{
		public ContactAddressController(IMediator mediator, IOptions<ApiSettings> apiSettings) : base(mediator, apiSettings)
		{
		}

		[HttpPost(Name = nameof(UpdateAddress))]
		[Authorize(Roles = UserRoles.Admin)]
		[ProducesResponseType(typeof(UpdateContactAddressResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> UpdateAddress([FromRoute] string countryCode, [FromBody] UpdateAddressForm form)
		{
			if (await Mediator.Send(new UpdateContactAddressCommand(form.Street, form.City, form.PostalCode, form.Country)))
			{
				return Ok(new UpdateContactAddressResponseModel(ApiSettings.ApiUrl, countryCode));
			}

			return CustomApiResponses.InternalServerError(new ExceptionResponseModel("Vyskytla se chyba při aktualizaci adresy"));
		}
	}
}
