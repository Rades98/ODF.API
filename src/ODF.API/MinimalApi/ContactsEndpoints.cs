using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ODF.API.FormFactories;
using ODF.API.Registration.SettingModels;
using ODF.API.RequestModels.Forms.Contacts;
using ODF.API.ResponseComposers.Contacts;
using ODF.API.ResponseModels.Contacts.Create;
using ODF.API.ResponseModels.Contacts.Delete;
using ODF.API.ResponseModels.Contacts.Update;
using ODF.API.ResponseModels.Exceptions;
using ODF.API.Responses;
using ODF.AppLayer.Consts;
using ODF.AppLayer.CQRS.Contact.Commands;
using ODF.AppLayer.CQRS.Contact.Queries;

namespace ODF.API.MinimalApi
{
	public static class ContactsEndpoints
	{
		public static WebApplication MapContactsEndpoints(this WebApplication app, IMediator mediator, ApiSettings apiSettings)
		{
			app.MapGet("/{countryCode}/contacts", async ([FromRoute] string countryCode, CancellationToken cancellationToken) =>
			{
				var contact = await mediator.Send(new GetContactQuery(countryCode), cancellationToken);

				return Results.Ok(ContactsResponseComposer.GetContactResponse(countryCode, apiSettings.ApiUrl, contact));
			});

			app.MapGet("/{countryCode}/contacts/redaction", [Authorize(Roles = UserRoles.Admin)] async ([FromRoute] string countryCode, CancellationToken cancellationToken) =>
			{
				var contact = await mediator.Send(new GetContactQuery(countryCode), cancellationToken);

				return Results.Ok(ContactsResponseComposer.GetRedactionResponse(countryCode, apiSettings.ApiUrl, contact));
			});

			app.MapPost("/{countryCode}/contacts", [Authorize(Roles = UserRoles.Admin)] async ([FromRoute] string countryCode, [FromBody] UpdateContactForm form, CancellationToken cancellationToken) =>
			{
				if (await mediator.Send(new UpdateContactCommand(form.EventName, form.EventManager, form.Email), cancellationToken))
				{
					return Results.Ok(new UpdateContactResponseModel(apiSettings.ApiUrl, countryCode));
				}

				return CustomApiResponses.InternalServerError(new ExceptionResponseModel("Vyskytla se chyba při aktualizaci kontaktu"));

			});

			app.MapPost("/{countryCode}/contacts/address", [Authorize(Roles = UserRoles.Admin)] async ([FromRoute] string countryCode, [FromBody] UpdateAddressForm form, CancellationToken cancellationToken) =>
			{
				if (await mediator.Send(new UpdateContactAddressCommand(form.Street, form.City, form.PostalCode, form.Country), cancellationToken))
				{
					return Results.Ok(new UpdateContactAddressResponseModel(apiSettings.ApiUrl, countryCode));
				}

				return CustomApiResponses.InternalServerError(new ExceptionResponseModel("Vyskytla se chyba při aktualizaci adresy"));
			});

			#region BankAcc

			app.MapPut("/{countryCode}/contacts/bankAcc", [Authorize(Roles = UserRoles.Admin)] async ([FromRoute] string countryCode, [FromBody] AddBankAccountForm form, CancellationToken cancellationToken) =>
			{
				var validationResponse = await mediator.Send(new AddBankAccountCommand(form.Bank, form.AccountId, form.IBAN), cancellationToken);

				if (validationResponse.IsOk)
				{
					return Results.Ok(new CreateContactBankAccResponseModel(apiSettings.ApiUrl, countryCode));
				}

				if (validationResponse.Errors.Any())
				{
					var resposneForm = ContactFormFactory.GetAddBankAcountForm(validationResponse.Errors, form.Bank, form.AccountId, form.IBAN);
					return Results.UnprocessableEntity(new CreateContactBankAccResponseModel(apiSettings.ApiUrl, countryCode, resposneForm));
				}

				return CustomApiResponses.InternalServerError(new ExceptionResponseModel("Vyskytla se chyba při přidávání bankovního účtu"));
			});

			app.MapDelete("/{countryCode}/contacts/bankAcc", [Authorize(Roles = UserRoles.Admin)] async ([FromRoute] string countryCode, [FromBody] RemoveBankAccountForm form, CancellationToken cancellationToken) =>
			{
				if (await mediator.Send(new RemoveBankAccountCommand(form.IBAN), cancellationToken))
				{
					return Results.Ok(new DeleteContactBankAccResponseModel(apiSettings.ApiUrl, countryCode));
				}

				return CustomApiResponses.InternalServerError(new ExceptionResponseModel("Vyskytla se chyba při mazání bankovního účtu"));
			});

			#endregion BankAcc

			#region Person

			app.MapPost("/{countryCode}/contacts/person", [Authorize(Roles = UserRoles.Admin)] async ([FromRoute] string countryCode, [FromBody] UpdateContactPersonForm form, CancellationToken cancellationToken) =>
			{

				if (await mediator.Send(new UpdateContactPersonCommand(form.Email, form.Title, form.Name, form.Surname, form.Roles, form.Base64Image, form.Id, form.Order), cancellationToken))
				{
					return Results.Ok(new UpdateContactPersonResponseModel(apiSettings.ApiUrl, countryCode));
				}

				return CustomApiResponses.InternalServerError(new ExceptionResponseModel("Vyskytla se chyba při aktualizaci kontaktní osoby"));
			});

			app.MapPut("/{countryCode}/contacts/person", [Authorize(Roles = UserRoles.Admin)] async ([FromRoute] string countryCode, [FromBody] AddContactPersonForm form, CancellationToken cancellationToken) =>
			{
				var validationResponse = await mediator.Send(new AddContactPersonCommand(form.Email, form.Title, form.Name, form.Surname, form.Roles, form.Base64Image), cancellationToken);

				if (validationResponse.IsOk)
				{
					return Results.Ok(new CreateContactPersonResponseModel(apiSettings.ApiUrl, countryCode));
				}

				if (validationResponse.Errors.Any())
				{
					var resposneForm = ContactFormFactory.GetAddContactPersonForm(validationResponse.Errors, form);
					return Results.UnprocessableEntity(new CreateContactPersonResponseModel(apiSettings.ApiUrl, countryCode, resposneForm));
				}

				return CustomApiResponses.InternalServerError(new ExceptionResponseModel("Vyskytla se chyba při tvorbě kontaktní osoby"));
			});

			app.MapDelete("/{countryCode}/contacts/person", [Authorize(Roles = UserRoles.Admin)] async ([FromRoute] string countryCode, [FromBody] RemoveContactPersonForm form, CancellationToken cancellationToken) =>
			{
				if (await mediator.Send(new RemoveContactPersonCommand(form.Id), cancellationToken))
				{
					return Results.Ok(new DeleteContactPersonResponseModel(apiSettings.ApiUrl, countryCode));
				}

				return CustomApiResponses.InternalServerError(new ExceptionResponseModel("Vyskytla se chyba při mazání kontaktní osoby"));
			});

			#endregion Person

			return app;
		}
	}
}
