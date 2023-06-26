using FluentValidation.Results;
using ODF.API.Extensions;
using ODF.API.RequestModels.Forms.Contacts;
using ODF.API.ResponseModels.Common.Forms;
using ODF.AppLayer.CQRS.Interfaces.Contact;
using ODF.Domain.Constants;

namespace ODF.API.FormComposers
{
	public static class ContactFormComposer
	{
		public static Form GetUpdateContactForm(UpdateContactForm contact, IEnumerable<ValidationFailure>? errors = null)
		{
			var form = new Form();
			form.AddMember(new("Název akce", nameof(IUpdateContact.EventName), FormValueTypes.Text, contact.EventName, true,
				errors?.GetErrorMessage(nameof(IUpdateContact.EventName))));

			form.AddMember(new("Pořadatel", nameof(IUpdateContact.EventManager), FormValueTypes.Text, contact.EventManager, true,
				errors?.GetErrorMessage(nameof(IUpdateContact.EventManager))));

			form.AddMember(new("e-mail", nameof(IUpdateContact.Email), FormValueTypes.Text, contact.Email, true,
				errors?.GetErrorMessage(nameof(IUpdateContact.Email))));

			return form;
		}

		public static Form GetUpdateAddressForm(UpdateAddressForm address, IEnumerable<ValidationFailure>? errors = null)
		{
			var form = new Form();

			form.AddMember(new("Město", nameof(IUpdateContactAddress.City), FormValueTypes.Text, address.City, true,
				errors?.GetErrorMessage(nameof(IUpdateContactAddress.City))));

			form.AddMember(new("Ulice", nameof(IUpdateContactAddress.Street), FormValueTypes.Text, address.Street, true,
				errors?.GetErrorMessage(nameof(IUpdateContactAddress.Street))));

			form.AddMember(new("PSČ", nameof(IUpdateContactAddress.PostalCode), FormValueTypes.Text, address.PostalCode, true,
				errors?.GetErrorMessage(nameof(IUpdateContactAddress.PostalCode))));

			form.AddMember(new("Země", nameof(IUpdateContactAddress.Country), FormValueTypes.Text, address.Country, true,
				errors?.GetErrorMessage(nameof(IUpdateContactAddress.Country))));

			return form;
		}

		public static Form GetAddBankAcountForm(AddBankAccountForm bankAcc, IEnumerable<ValidationFailure>? errors = null)
		{
			var form = new Form();
			form.AddMember(new("Banka", nameof(IAddBankAccount.Bank), FormValueTypes.Text, bankAcc.Bank, true,
				errors?.GetErrorMessage(nameof(IAddBankAccount.Bank))));

			form.AddMember(new("Číslo účtu", nameof(IAddBankAccount.AccountId), FormValueTypes.Text, bankAcc.AccountId, true,
				errors?.GetErrorMessage(nameof(IAddBankAccount.AccountId))));

			form.AddMember(new("IBAN", nameof(IAddBankAccount.IBAN), FormValueTypes.Text, bankAcc.IBAN, true,
				errors?.GetErrorMessage(nameof(IAddBankAccount.IBAN))));

			return form;
		}

		public static Form GetRemoveBankAcountForm(RemoveBankAccountForm iban, IEnumerable<ValidationFailure>? errors = null)
		{
			var form = new Form();
			form.AddMember(new("IBAN", nameof(IRemoveBankAccount.IBAN), FormValueTypes.Text, iban, true,
				errors?.GetErrorMessage(nameof(IRemoveBankAccount.IBAN))));

			return form;
		}

		public static Form GetAddContactPersonForm(AddContactPersonForm? reqForm = null, IEnumerable<ValidationFailure>? errors = null)
		{
			var form = new Form();
			form.AddMember(new("e-mail", nameof(IAddContactPerson.Email), FormValueTypes.Text, reqForm?.Email ?? "", true,
				errors?.GetErrorMessage(nameof(IAddContactPerson.Email))));

			form.AddMember(new("Titul", nameof(IAddContactPerson.Title), FormValueTypes.Text, reqForm?.Title ?? "", true,
				errors?.GetErrorMessage(nameof(IAddContactPerson.Title))));

			form.AddMember(new("Jméno", nameof(IAddContactPerson.Name), FormValueTypes.Text, reqForm?.Name ?? "", true,
				errors?.GetErrorMessage(nameof(IAddContactPerson.Name))));

			form.AddMember(new("Příjmení", nameof(IAddContactPerson.Surname), FormValueTypes.Text, reqForm?.Surname ?? "text", true,
				errors?.GetErrorMessage(nameof(IAddContactPerson.Surname))));

			form.AddMember(new("Obrázek", nameof(IAddContactPerson.Base64Image), FormValueTypes.Text, reqForm?.Base64Image ?? "text", true,
				errors?.GetErrorMessage(nameof(IAddContactPerson.Base64Image))));

			form.AddMember(new("Role", nameof(IAddContactPerson.Roles), FormValueTypes.Text, reqForm?.Roles ?? Enumerable.Empty<string>(), true,
				errors?.GetErrorMessage(nameof(IAddContactPerson.Roles))));

			return form;
		}

		public static Form GetUpdateContactPersonForm(UpdateContactPersonForm person, IEnumerable<ValidationFailure>? errors = null)
		{
			var form = new Form();

			form.AddMember(new("e-mail", nameof(IUpdateContactPerson.Email), FormValueTypes.Text, person.Email, true,
				errors?.GetErrorMessage(nameof(IUpdateContactPerson.Email))));

			form.AddMember(new("Titul", nameof(IUpdateContactPerson.Title), FormValueTypes.Text, person.Title, true,
				errors?.GetErrorMessage(nameof(IUpdateContactPerson.Title))));

			form.AddMember(new("Jméno", nameof(IUpdateContactPerson.Name), FormValueTypes.Text, person.Name, true,
				errors?.GetErrorMessage(nameof(IUpdateContactPerson.Name))));

			form.AddMember(new("Příjmení", nameof(IUpdateContactPerson.Surname), FormValueTypes.Text, person.Surname, true,
				errors?.GetErrorMessage(nameof(IUpdateContactPerson.Surname))));

			form.AddMember(new("Obrázek", nameof(IUpdateContactPerson.Base64Image), FormValueTypes.Text, person.Base64Image, true,
				errors?.GetErrorMessage(nameof(IUpdateContactPerson.Base64Image))));

			form.AddMember(new("Role", nameof(IUpdateContactPerson.Roles), FormValueTypes.TextArray, person.Roles, true,
				errors?.GetErrorMessage(nameof(IUpdateContactPerson.Roles))));

			form.AddMember(new("Id", nameof(IUpdateContactPerson.Id), FormValueTypes.Text, person.Id, false,
				errors?.GetErrorMessage(nameof(IUpdateContactPerson.Id))));

			form.AddMember(new("Order", nameof(IUpdateContactPerson.Order), FormValueTypes.Number, person.Order, true,
				errors?.GetErrorMessage(nameof(IUpdateContactPerson.Order))));

			return form;
		}

		public static Form GetRemoveContactPersonForm(RemoveContactPersonForm person, IEnumerable<ValidationFailure>? errors = null)
		{
			var form = new Form();
			form.AddMember(new("Id", nameof(IRemoveContactPerson.Id), FormValueTypes.Text, person.Id, false,
				errors?.GetErrorMessage(nameof(IRemoveContactPerson.Id))));

			return form;
		}
	}
}
