using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ODF.API.Controllers.Base;
using ODF.API.Registration.SettingModels;
using ODF.API.ResponseComposers.Contacts;
using ODF.API.ResponseModels.Redaction;
using ODF.AppLayer.Consts;
using ODF.AppLayer.CQRS.Contact.Queries;

namespace ODF.API.Controllers.Contacts
{
	public class ContactsRedactionController : BaseController
	{
		public ContactsRedactionController(IMediator mediator, IOptions<ApiSettings> apiSettings) : base(mediator, apiSettings)
		{
		}

		[HttpGet(Name = nameof(GetContactsRedaction))]
		[Authorize(Roles = UserRoles.Admin)]
		[ProducesResponseType(typeof(RedactionResponseModel), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetContactsRedaction([FromRoute] string countryCode)
		{
			var contact = await Mediator.Send(new GetContactQuery(countryCode));

			return Ok(ContactsResponseComposer.GetRedactionResponse(countryCode, ApiSettings.ApiUrl, contact));
		}
	}
}
