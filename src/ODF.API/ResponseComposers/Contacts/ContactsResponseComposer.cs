using ODF.API.FormFactories;
using ODF.API.ResponseModels.Common;
using ODF.API.ResponseModels.Contacts.GetContacts;
using ODF.API.ResponseModels.Contacts.Redaction;
using ODF.AppLayer.Dtos.ContactDtos;

namespace ODF.API.ResponseComposers.Contacts
{
	internal static class ContactsResponseComposer
	{
		internal static ContactResponseModel GetContactResponse(string countryCode, string baseUrl, ContactDto model)
		{
			var responseModel = new ContactResponseModel(baseUrl, countryCode);

			responseModel.ContactPersons = model.ContactPersons.Select(per => new GetContactPersonResponseModel()
			{
				Base64Image = per.Base64Image,
				Email = per.Email,
				Name = per.Name,
				Title = per.Title,
				Surname = per.Surname,
				Roles = per.Roles,
			});

			responseModel.BankAccounts = model.BankAccounts.Select(acc => new GetBankAccountResponseModel()
			{
				AccountId = acc.AccountId,
				AccountIdTranslation = acc.AccountIdTranslation,
				Bank = acc.Bank,
				BankTranslation = acc.BankTranslation,
				IBAN = acc.IBAN,
				IBANTranslation = acc.IBANTranslation,
			});

			responseModel.Address = new GetAddressResponseModel()
			{
				City = model.Address.City,
				Country = model.Address.Country,
				PostalCode = model.Address.PostalCode,
				Street = model.Address.Street,
			};

			responseModel.EventName = model.EventName;
			responseModel.EventManager = model.EventManager;
			responseModel.EventManagerTranslation = model.EventManagerTranslation;
			responseModel.EmailTranslation = model.EmailTranslation;
			responseModel.Email = model.Email;

			return responseModel;
		}

		internal static GetContactRedactionNavigationResponseModel GetRedactionResponse(string countryCode, string baseUrl, ContactDto model)
		{
			var responseModel = new GetContactRedactionNavigationResponseModel(baseUrl, countryCode);

			responseModel.UpdateContacts = new NamedAction($"{baseUrl}/{countryCode}/contacts", "Upravit kontakt", "updateContact", HttpMethods.Post, ContactFormFactory.GetUpdateContactForm(model));
			responseModel.UpdateAddress = new NamedAction($"{baseUrl}/{countryCode}/contacts/address", "Upravit adresu", "updateAddress", HttpMethods.Post, ContactFormFactory.GetUpdateAddressForm(model.Address));

			// Bank acc
			responseModel.AddBankAccount = new NamedAction($"{baseUrl}/{countryCode}/contacts/bankAcc", "Přidat bankovní účet", "addBankAccount", HttpMethods.Put, ContactFormFactory.GetAddBankAcountForm());
			
			responseModel.RemoveBankAccountActions = model.BankAccounts.Select(bankAcc
				=> new NamedAction($"{baseUrl}/{countryCode}/contacts/bankAcc", $"Smazat bankovní účet {bankAcc.IBAN}", "removeBankAccount", HttpMethods.Delete, ContactFormFactory.GetRemoveBankAcountForm(bankAcc.IBAN)));

			// Contact persons
			responseModel.AddContactPerson = new NamedAction($"{baseUrl}/{countryCode}/contacts/person", "Přidat kontaktní osobu", "addContactPerson", HttpMethods.Put, ContactFormFactory.GetAddContactPersonForm());

			responseModel.ContactPersons = model.ContactPersons.Select(person => new GetContactPersonRedactionResponseModel()
			{
				Title = person.Title,
				Name = person.Name,
				Surname = person.Surname,
				DeleteContactPerson = new NamedAction($"{baseUrl}/{countryCode}/contacts/person", $"Smazat", "removeContactPerson", HttpMethods.Post, ContactFormFactory.GetRemoveContactPersonForm(person.Id)),
				UpdateContactPerson = new NamedAction($"{baseUrl}/{countryCode}/contacts/person", $"Upravit", "updateContactPerson", HttpMethods.Post, ContactFormFactory.GetUpdateContactPersonForm(person))
			});


			return responseModel;

		}
	}
}
