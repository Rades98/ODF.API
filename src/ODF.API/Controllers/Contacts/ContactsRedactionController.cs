﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using ODF.API.Controllers.Base;
using ODF.API.FormFactories;
using ODF.API.Registration.SettingModels;
using ODF.API.ResponseModels.Contacts.Redaction;
using ODF.API.ResponseModels.Redaction;
using ODF.AppLayer.Consts;
using ODF.AppLayer.CQRS.Contact.Queries;

namespace ODF.API.Controllers.Contacts
{
	public class ContactsRedactionController : BaseController
	{
		public ContactsRedactionController(IMediator mediator, IOptions<ApiSettings> apiSettings, IActionDescriptorCollectionProvider adcp) : base(mediator, apiSettings, adcp)
		{
		}

		[HttpGet(Name = nameof(GetContactsRedaction))]
		[Authorize(Roles = UserRoles.Admin)]
		[ProducesResponseType(typeof(RedactionResponseModel), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetContactsRedaction([FromRoute] string countryCode)
		{
			var contact = await Mediator.Send(new GetContactQuery(countryCode));

			var responseModel = new GetContactRedactionNavigationResponseModel();

			responseModel.UpdateContacts = GetNamedAction(nameof(ContactsController.UpdateContact), "Upravit kontakt",
				"updateContact", ContactFormFactory.GetUpdateContactForm(contact));

			responseModel.UpdateAddress = GetNamedAction(nameof(ContactAddressController.UpdateAddress), "Upravit adresu",
				"updateAddress", ContactFormFactory.GetUpdateAddressForm(contact.Address));

			// Bank acc
			responseModel.AddBankAccount = GetNamedAction(nameof(ContactBankAccountsController.AddBankAccount),
				"Přidat bankovní účet", "addBankAccount", ContactFormFactory.GetAddBankAcountForm());

			responseModel.RemoveBankAccountActions = contact.BankAccounts.Select(bankAcc
				=> GetNamedAction(nameof(ContactBankAccountsController.RemoveBankAccount),
				$"Smazat bankovní účet {bankAcc.IBAN}", "removeBankAccount",
				ContactFormFactory.GetRemoveBankAcountForm(bankAcc.IBAN)));

			// Contact persons
			responseModel.AddContactPerson = GetNamedAction(nameof(ContactPersonsController.AddContactPerson),
				"Přidat kontaktní osobu", "addContactPerson", ContactFormFactory.GetAddContactPersonForm());

			responseModel.ContactPersons = contact.ContactPersons.Select(person => new GetContactPersonRedactionResponseModel()
			{
				Title = person.Title,
				Name = person.Name,
				Surname = person.Surname,
				DeleteContactPerson = GetNamedAction(nameof(ContactPersonsController.RemoveContactPerson),
					"Smazat", "removeContactPerson", ContactFormFactory.GetRemoveContactPersonForm(person.Id)),
				UpdateContactPerson = GetNamedAction(nameof(ContactPersonsController.UpdateContactPerson),
					"Upravit", "updateContactPerson", ContactFormFactory.GetUpdateContactPersonForm(person))
			});

			return Ok(responseModel);
		}
	}
}
