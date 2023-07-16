using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using ODF.API.Attributes.HtttpMethodAttributes;
using ODF.API.Controllers.Base;
using ODF.API.Extensions.MappingExtensions;
using ODF.API.FormComposers;
using ODF.API.ResponseModels.Contacts.Redaction;
using ODF.API.ResponseModels.Redaction;
using ODF.AppLayer.CQRS.Contact.Queries;
using ODF.AppLayer.Services.Interfaces;
using ODF.Domain.Constants;
using ODF.Domain.SettingModels;

namespace ODF.API.Controllers.Contacts
{
	public class ContactRedactionController : BaseController
	{
		public ContactRedactionController(IMediator mediator, IOptions<ApiSettings> apiSettings, IActionDescriptorCollectionProvider adcp, ITranslationsProvider translationsProvider)
			: base(mediator, apiSettings, adcp, translationsProvider)
		{
		}

		[HttpGet(Name = nameof(GetContactsRedaction))]
		[Authorize(Roles = UserRoles.Admin)]
		[CountryCodeFilter("cz")]
		[ProducesResponseType(typeof(RedactionResponseModel), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetContactsRedaction(CancellationToken cancellationToken)
		{
			var contact = await Mediator.Send(new GetContactQuery(CountryCode), cancellationToken);

			var responseModel = new GetContactRedactionNavigationResponseModel();

			responseModel.UpdateContacts = GetNamedAction(nameof(ContactController.UpdateContact), "Upravit kontakt",
				"updateContact", ContactFormComposer.GetUpdateContactForm(contact.ToForm()));

			responseModel.UpdateAddress = GetNamedAction(nameof(ContactAddressController.UpdateAddress), "Upravit adresu",
				"updateAddress", ContactFormComposer.GetUpdateAddressForm(contact.Address.ToForm()));

			// Bank acc
			responseModel.AddBankAccount = GetNamedAction(nameof(ContactBankAccountsController.AddBankAccount),
				"Přidat bankovní účet", "addBankAccount", ContactFormComposer.GetAddBankAcountForm(new()));

			responseModel.RemoveBankAccountActions = contact.BankAccounts.Select(bankAcc
				=> GetNamedAction(nameof(ContactBankAccountsController.RemoveBankAccount),
				$"Smazat bankovní účet {bankAcc.IBAN}", "removeBankAccount",
				ContactFormComposer.GetRemoveBankAcountForm(new() { IBAN = bankAcc.IBAN })));

			responseModel.AddContactPerson = GetNamedAction(nameof(ContactPersonsController.AddContactPerson),
				"Přidat kontaktní osobu", "addContactPerson", ContactFormComposer.GetAddContactPersonForm());

			// Contact persons
			responseModel.ContactPersons = contact.ContactPersons.Select(person => new GetContactPersonRedactionResponseModel()
			{
				Title = person.Title,
				Name = person.Name,
				Surname = person.Surname,
				Email = person.Email,

				DeleteContactPerson = GetNamedAction(nameof(ContactPersonsController.RemoveContactPerson),
					"Smazat", "removeContactPerson", ContactFormComposer.GetRemoveContactPersonForm(new() { Id = person.Id })),

				UpdateContactPerson = GetNamedAction(nameof(ContactPersonsController.UpdateContactPerson),
					"Upravit", "updateContactPerson", ContactFormComposer.GetUpdateContactPersonForm(new()
					{
						Base64Image = person.Base64Image,
						Email = person.Email,
						Id = person.Id,
						Name = person.Name,
						Surname = person.Surname,
						Order = person.Order,
						Roles = person.Roles,
						Title = person.Title,
					})),
			});

			return Ok(responseModel);
		}
	}
}
