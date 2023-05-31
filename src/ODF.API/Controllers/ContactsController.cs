using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ODF.API.FormFactories;
using ODF.API.Registration.SettingModels;
using ODF.API.RequestModels.Forms.Contacts;
using ODF.API.ResponseComposers.Contacts;
using ODF.API.ResponseModels.About;
using ODF.API.ResponseModels.Contacts.Create;
using ODF.API.ResponseModels.Contacts.Delete;
using ODF.API.ResponseModels.Contacts.GetContacts;
using ODF.API.ResponseModels.Contacts.Update;
using ODF.API.ResponseModels.Exceptions;
using ODF.API.ResponseModels.Redaction;
using ODF.API.Responses;
using ODF.AppLayer.Consts;
using ODF.AppLayer.CQRS.Contact.Commands;
using ODF.AppLayer.CQRS.Contact.Queries;
using ODF.AppLayer.CQRS.Translations.Queries;
using System.Data;

namespace ODF.API.Controllers
{
    public class ContactsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ApiSettings _settings;

        public ContactsController(IMediator mediator, IOptions<ApiSettings> apiSettings)
        {
            _mediator = mediator;
            _settings = apiSettings.Value;
        }

        #region MainContacts
        [HttpGet("/{countryCode}/contacts")]
        [ProducesResponseType(typeof(ContactResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetContacts([FromRoute] string countryCode)
        {
            var contact = await _mediator.Send(new GetContactQuery(countryCode));

            return Ok(ContactsResponseComposer.GetContactResponse(countryCode, _settings.ApiUrl, contact));
        }

        [HttpGet("/{countryCode}/contacts/redaction")]
        [Authorize(Roles = UserRoles.Admin)]
        [ProducesResponseType(typeof(RedactionResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRedactionContacts([FromRoute] string countryCode)
        {
            var contact = await _mediator.Send(new GetContactQuery(countryCode));

            return Ok(ContactsResponseComposer.GetRedactionResponse(countryCode, _settings.ApiUrl, contact));
        }

        [HttpPost("/{countryCode}/contacts")]
        [Authorize(Roles = UserRoles.Admin)]
        [ProducesResponseType(typeof(UpdateContactResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateContact([FromRoute] string countryCode, [FromBody] UpdateContactForm form)
        {
            if (await _mediator.Send(new UpdateContactCommand(form.EventName, form.EventManager, form.Email)))
            {
                return Ok(new UpdateContactResponseModel(_settings.ApiUrl, countryCode));
            }

            return (IActionResult)CustomApiResponses.InternalServerError(new ExceptionResponseModel("Vyskytla se chyba při aktualizaci kontaktu"));
        }

        [HttpPost("/{countryCode}/contacts/address")]
        [Authorize(Roles = UserRoles.Admin)]
        [ProducesResponseType(typeof(UpdateContactAddressResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAddress([FromRoute] string countryCode, [FromBody] UpdateAddressForm form)
        {
            if (await _mediator.Send(new UpdateContactAddressCommand(form.Street, form.City, form.PostalCode, form.Country)))
            {
                return Ok(new UpdateContactAddressResponseModel(_settings.ApiUrl, countryCode));
            }

            return (IActionResult)CustomApiResponses.InternalServerError(new ExceptionResponseModel("Vyskytla se chyba při aktualizaci adresy"));
        }
        #endregion MainContacts

        #region BankAccount
        [HttpPut("/{countryCode}/contacts/bankAcc")]
        [Authorize(Roles = UserRoles.Admin)]
        [ProducesResponseType(typeof(CreateContactBankAccResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CreateContactBankAccResponseModel), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddBankAccount([FromRoute] string countryCode, [FromBody] AddBankAccountForm form, CancellationToken cancellationToken)
        {
            var validationResponse = await _mediator.Send(new AddBankAccountCommand(form.Bank, form.AccountId, form.IBAN), cancellationToken);

            if (validationResponse.IsOk)
            {
                return Ok(new CreateContactBankAccResponseModel(_settings.ApiUrl, countryCode));
            }

            if (validationResponse.Errors.Any())
            {
                var responseForm = ContactFormFactory.GetAddBankAcountForm(validationResponse.Errors, form.Bank, form.AccountId, form.IBAN);
                return UnprocessableEntity(new CreateContactBankAccResponseModel(_settings.ApiUrl, countryCode, responseForm));
            }

            return (IActionResult)CustomApiResponses.InternalServerError(new ExceptionResponseModel("Vyskytla se chyba při přidávání bankovního účtu"));
        }

        [HttpDelete("/{countryCode}/contacts/bankAcc")]
        [Authorize(Roles = UserRoles.Admin)]
        [ProducesResponseType(typeof(DeleteContactBankAccResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveBankAccount([FromRoute] string countryCode, [FromBody] RemoveBankAccountForm form, CancellationToken cancellationToken)
        {
            if (await _mediator.Send(new RemoveBankAccountCommand(form.IBAN), cancellationToken))
            {
                return Ok(new DeleteContactBankAccResponseModel(_settings.ApiUrl, countryCode));
            }

            return (IActionResult)CustomApiResponses.InternalServerError(new ExceptionResponseModel("Vyskytla se chyba při mazání bankovního účtu"));
        }
        #endregion BankAccount

        #region Person
        [HttpPost("/{countryCode}/contacts/person")]
        [Authorize(Roles = UserRoles.Admin)]
        [ProducesResponseType(typeof(UpdateContactPersonResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateContactPerson([FromRoute] string countryCode, [FromBody] UpdateContactPersonForm form, CancellationToken cancellationToken)
        {
            if (await _mediator.Send(new UpdateContactPersonCommand(form.Email, form.Title, form.Name, form.Surname, form.Roles, form.Base64Image, form.Id, form.Order), cancellationToken))
            {
                return Ok(new UpdateContactPersonResponseModel(_settings.ApiUrl, countryCode));
            }

            return (IActionResult)CustomApiResponses.InternalServerError(new ExceptionResponseModel("Vyskytla se chyba při aktualizaci kontaktní osoby"));
        }

        [HttpPut("/{countryCode}/contacts/person")]
        [Authorize(Roles = UserRoles.Admin)]
        [ProducesResponseType(typeof(CreateContactPersonResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CreateContactPersonResponseModel), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddContactPerson([FromRoute] string countryCode, [FromBody] AddContactPersonForm form, CancellationToken cancellationToken)
        {
            var validationResponse = await _mediator.Send(new AddContactPersonCommand(form.Email, form.Title, form.Name, form.Surname, form.Roles, form.Base64Image), cancellationToken);

            if (validationResponse.IsOk)
            {
                return Ok(new CreateContactPersonResponseModel(_settings.ApiUrl, countryCode));
            }

            /* 2RADEK: Toto je ta validacia, ktoru by si chcel vsade?? */
            if (validationResponse.Errors.Any())
            {
                var responseForm = ContactFormFactory.GetAddContactPersonForm(validationResponse.Errors, form);
                return UnprocessableEntity(new CreateContactPersonResponseModel(_settings.ApiUrl, countryCode, responseForm));
            }

            return (IActionResult)CustomApiResponses.InternalServerError(new ExceptionResponseModel("Vyskytla se chyba při tvorbě kontaktní osoby"));
        }

        [HttpDelete("/{countryCode}/contacts/person")]
        [Authorize(Roles = UserRoles.Admin)]
        [ProducesResponseType(typeof(DeleteContactPersonResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveContactPerson([FromRoute] string countryCode, [FromBody] RemoveContactPersonForm form, CancellationToken cancellationToken)
        {
            if (await _mediator.Send(new RemoveContactPersonCommand(form.Id), cancellationToken))
            {
                return Ok(new DeleteContactPersonResponseModel(_settings.ApiUrl, countryCode));
            }

            return (IActionResult)CustomApiResponses.InternalServerError(new ExceptionResponseModel("Vyskytla se chyba při mazání kontaktní osoby"));
        }

        #endregion Person
    }
}
