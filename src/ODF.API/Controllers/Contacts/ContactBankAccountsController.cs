using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using ODF.API.Attributes.HtttpMethodAttributes;
using ODF.API.Controllers.Base;
using ODF.API.FormFactories;
using ODF.API.RequestModels.Forms.Contacts;
using ODF.API.ResponseModels.Contacts.Create;
using ODF.API.ResponseModels.Contacts.Delete;
using ODF.API.ResponseModels.Exceptions;
using ODF.API.Responses;
using ODF.AppLayer.Consts;
using ODF.AppLayer.CQRS.Contact.Commands;
using ODF.AppLayer.Services.Interfaces;
using ODF.Domain.SettingModels;

namespace ODF.API.Controllers.Contacts
{
	public class ContactBankAccountsController : BaseController
	{
		public ContactBankAccountsController(IMediator mediator, IOptions<ApiSettings> apiSettings, IActionDescriptorCollectionProvider adcp, ITranslationsProvider translationsProvider) : base(mediator, apiSettings, adcp, translationsProvider)
		{
		}

		[HttpPut(Name = nameof(AddBankAccount))]
		[Authorize(Roles = UserRoles.Admin)]
		[CountryCodeFilter("cz")]
		[ProducesResponseType(typeof(CreateContactBankAccResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(CreateContactBankAccResponseModel), StatusCodes.Status422UnprocessableEntity)]
		[ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> AddBankAccount([FromBody] AddBankAccountForm form, CancellationToken cancellationToken)
		{
			var validationResponse = await Mediator.Send(new AddBankAccountCommand(form.Bank, form.AccountId, form.IBAN), cancellationToken);

			if (validationResponse.IsOk)
			{
				return Ok(new CreateContactBankAccResponseModel());
			}

			if (validationResponse.Errors.Any())
			{
				var responseForm = ContactFormFactory.GetAddBankAcountForm(validationResponse.Errors, form.Bank, form.AccountId, form.IBAN);
				return UnprocessableEntity(new CreateContactBankAccResponseModel(responseForm));
			}

			return CustomApiResponses.InternalServerError(new ExceptionResponseModel("Vyskytla se chyba při přidávání bankovního účtu"));
		}

		[HttpDelete(Name = nameof(RemoveBankAccount))]
		[Authorize(Roles = UserRoles.Admin)]
		[CountryCodeFilter("cz")]
		[ProducesResponseType(typeof(DeleteContactBankAccResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> RemoveBankAccount([FromBody] RemoveBankAccountForm form, CancellationToken cancellationToken)
		{
			if (await Mediator.Send(new RemoveBankAccountCommand(form.IBAN), cancellationToken))
			{
				return Ok(new DeleteContactBankAccResponseModel());
			}

			return CustomApiResponses.InternalServerError(new ExceptionResponseModel("Vyskytla se chyba při mazání bankovního účtu")); // TODO add validation with form
		}
	}
}
