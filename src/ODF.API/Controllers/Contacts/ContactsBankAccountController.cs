using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ODF.API.Controllers.Base;
using ODF.API.FormFactories;
using ODF.API.Registration.SettingModels;
using ODF.API.RequestModels.Forms.Contacts;
using ODF.API.ResponseModels.Contacts.Create;
using ODF.API.ResponseModels.Contacts.Delete;
using ODF.API.ResponseModels.Exceptions;
using ODF.API.Responses;
using ODF.AppLayer.Consts;
using ODF.AppLayer.CQRS.Contact.Commands;

namespace ODF.API.Controllers.Contacts
{
	public class ContactsBankAccountController : BaseController
	{
		public ContactsBankAccountController(IMediator mediator, IOptions<ApiSettings> apiSettings) : base(mediator, apiSettings)
		{
		}

		[HttpPut("/{countryCode}/contacts/bankAcc")]
		[Authorize(Roles = UserRoles.Admin)]
		[ProducesResponseType(typeof(CreateContactBankAccResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(CreateContactBankAccResponseModel), StatusCodes.Status422UnprocessableEntity)]
		[ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> AddBankAccount([FromRoute] string countryCode, [FromBody] AddBankAccountForm form, CancellationToken cancellationToken)
		{
			var validationResponse = await Mediator.Send(new AddBankAccountCommand(form.Bank, form.AccountId, form.IBAN), cancellationToken);

			if (validationResponse.IsOk)
			{
				return Ok(new CreateContactBankAccResponseModel(ApiSettings.ApiUrl, countryCode));
			}

			if (validationResponse.Errors.Any())
			{
				var responseForm = ContactFormFactory.GetAddBankAcountForm(validationResponse.Errors, form.Bank, form.AccountId, form.IBAN);
				return UnprocessableEntity(new CreateContactBankAccResponseModel(ApiSettings.ApiUrl, countryCode, responseForm));
			}

			return CustomApiResponses.InternalServerError(new ExceptionResponseModel("Vyskytla se chyba při přidávání bankovního účtu"));
		}

		[HttpDelete("/{countryCode}/contacts/bankAcc")]
		[Authorize(Roles = UserRoles.Admin)]
		[ProducesResponseType(typeof(DeleteContactBankAccResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> RemoveBankAccount([FromRoute] string countryCode, [FromBody] RemoveBankAccountForm form, CancellationToken cancellationToken)
		{
			if (await Mediator.Send(new RemoveBankAccountCommand(form.IBAN), cancellationToken))
			{
				return Ok(new DeleteContactBankAccResponseModel(ApiSettings.ApiUrl, countryCode));
			}

			return CustomApiResponses.InternalServerError(new ExceptionResponseModel("Vyskytla se chyba při mazání bankovního účtu"));
		}
	}
}
