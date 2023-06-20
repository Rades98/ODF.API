using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using ODF.API.Attributes.HtttpMethodAttributes;
using ODF.API.Controllers.Base;
using ODF.API.FormFactories;
using ODF.API.ResponseModels.Contacts.Redaction;
using ODF.API.ResponseModels.Redaction;
using ODF.AppLayer.Consts;
using ODF.AppLayer.CQRS.Contact.Queries;
using ODF.AppLayer.Services.Interfaces;
using ODF.Domain.SettingModels;

namespace ODF.API.Controllers.Contacts
{
	public class ContactsRedactionController : BaseController
	{
		public ContactsRedactionController(IMediator mediator, IOptions<ApiSettings> apiSettings, IActionDescriptorCollectionProvider adcp, ITranslationsProvider translationsProvider)
			: base(mediator, apiSettings, adcp, translationsProvider)
		{
		}

		[HttpGet(Name = nameof(GetContactsRedaction))]
		[Authorize(Roles = UserRoles.Admin)]
		[CountryCodeFilter("cz")]
		[ProducesResponseType(typeof(RedactionResponseModel), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetContactsRedaction([FromRoute] string countryCode)
		{
			var contact = await Mediator.Send(new GetContactQuery(countryCode));

			var responseModel = new GetContactRedactionNavigationResponseModel();

			responseModel.UpdateContacts = GetNamedAction(nameof(ContactsController.UpdateContact), "Upravit kontakt",
				"updateContact", ContactFormFactory.GetUpdateContactForm(new() { Email = contact.Email, EventManager = contact.EventManager, EventName = contact.EventName }));

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
					"Upravit", "updateContactPerson", ContactFormFactory.GetUpdateContactPersonForm(new()
					{
						Base64Image = person.Base64Image,
						Email = person.Email,
						Id = person.Id,
						Name = person.Name,
						Surname = person.Surname,
						Order = person.Order,
						Roles = person.Roles,
						Title = person.Title,
					}))
			});

			return Ok(responseModel);
		}
	}
}
