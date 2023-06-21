using FluentValidation.Results;
using ODF.API.Extensions;
using ODF.API.RequestModels.Forms.Contacts;
using ODF.API.ResponseModels.Common.Forms;
using ODF.Domain.Constants;

namespace ODF.API.FormFactories
{
	public static class ContactFormFactory
	{
		public static Form GetUpdateContactForm(UpdateContactForm contact, IEnumerable<ValidationFailure>? errors = null)
		{
			var form = new Form();
			form.AddMember(new("Název akce", nameof(UpdateContactForm.EventName), FormValueTypes.Text, contact.EventName, true,
				errors?.GetErrorMessage(nameof(UpdateContactForm.EventName))));

			form.AddMember(new("Pořadatel", nameof(UpdateContactForm.EventManager), FormValueTypes.Text, contact.EventManager, true,
				errors?.GetErrorMessage(nameof(UpdateContactForm.EventManager))));

			form.AddMember(new("e-mail", nameof(UpdateContactForm.Email), FormValueTypes.Text, contact.Email, true,
				errors?.GetErrorMessage(nameof(UpdateContactForm.Email))));

			return form;
		}

		public static Form GetUpdateAddressForm(UpdateAddressForm address, IEnumerable<ValidationFailure>? errors = null)
		{
			var form = new Form();

			form.AddMember(new("Město", nameof(UpdateAddressForm.City), FormValueTypes.Text, address.City, true,
				errors?.GetErrorMessage(nameof(UpdateAddressForm.City))));

			form.AddMember(new("Ulice", nameof(UpdateAddressForm.Street), FormValueTypes.Text, address.Street, true,
				errors?.GetErrorMessage(nameof(UpdateAddressForm.Street))));

			form.AddMember(new("PSČ", nameof(UpdateAddressForm.PostalCode), FormValueTypes.Text, address.PostalCode, true,
				errors?.GetErrorMessage(nameof(UpdateAddressForm.PostalCode))));

			form.AddMember(new("Země", nameof(UpdateAddressForm.Country), FormValueTypes.Text, address.Country, true,
				errors?.GetErrorMessage(nameof(UpdateAddressForm.Country))));

			return form;
		}

		public static Form GetAddBankAcountForm(AddBankAccountForm bankAcc, IEnumerable<ValidationFailure>? errors = null)
		{
			var form = new Form();
			form.AddMember(new("Banka", nameof(AddBankAccountForm.Bank), FormValueTypes.Text, bankAcc.Bank, true,
				errors?.GetErrorMessage(nameof(AddBankAccountForm.Bank))));

			form.AddMember(new("Číslo účtu", nameof(AddBankAccountForm.AccountId), FormValueTypes.Text, bankAcc.AccountId, true,
				errors?.GetErrorMessage(nameof(AddBankAccountForm.AccountId))));

			form.AddMember(new("IBAN", nameof(AddBankAccountForm.IBAN), FormValueTypes.Text, bankAcc.IBAN, true,
				errors?.GetErrorMessage(nameof(AddBankAccountForm.IBAN))));

			return form;
		}

		public static Form GetRemoveBankAcountForm(string iban, IEnumerable<ValidationFailure>? errors = null)
		{
			var form = new Form();
			form.AddMember(new("IBAN", nameof(AddBankAccountForm.IBAN), FormValueTypes.Text, iban, true,
				errors?.GetErrorMessage(nameof(AddBankAccountForm.IBAN))));

			return form;
		}

		public static Form GetAddContactPersonForm(AddContactPersonForm? reqForm = null, IEnumerable<ValidationFailure>? errors = null)
		{
			var form = new Form();
			form.AddMember(new("e-mail", nameof(AddContactPersonForm.Email), FormValueTypes.Text, reqForm?.Email ?? "", true,
				errors?.GetErrorMessage(nameof(AddContactPersonForm.Email))));

			form.AddMember(new("Titul", nameof(AddContactPersonForm.Title), FormValueTypes.Text, reqForm?.Title ?? "", true,
				errors?.GetErrorMessage(nameof(AddContactPersonForm.Title))));

			form.AddMember(new("Jméno", nameof(AddContactPersonForm.Name), FormValueTypes.Text, reqForm?.Name ?? "", true,
				errors?.GetErrorMessage(nameof(AddContactPersonForm.Name))));

			form.AddMember(new("Příjmení", nameof(AddContactPersonForm.Surname), FormValueTypes.Text, reqForm?.Surname ?? "text", true,
				errors?.GetErrorMessage(nameof(AddContactPersonForm.Surname))));

			form.AddMember(new("Obrázek", nameof(AddContactPersonForm.Base64Image), FormValueTypes.Text, reqForm?.Base64Image ?? "text", true,
				errors?.GetErrorMessage(nameof(AddContactPersonForm.Base64Image))));

			form.AddMember(new("Role", nameof(AddContactPersonForm.Roles), FormValueTypes.Text, reqForm?.Roles ?? Enumerable.Empty<string>(), true,
				errors?.GetErrorMessage(nameof(AddContactPersonForm.Roles))));

			return form;
		}

		public static Form GetUpdateContactPersonForm(UpdateContactPersonForm person, IEnumerable<ValidationFailure>? errors = null)
		{
			var form = new Form();

			form.AddMember(new("e-mail", nameof(UpdateContactPersonForm.Email), FormValueTypes.Text, person.Email, true,
				errors?.GetErrorMessage(nameof(UpdateContactPersonForm.Email))));

			form.AddMember(new("Titul", nameof(UpdateContactPersonForm.Title), FormValueTypes.Text, person.Title, true,
				errors?.GetErrorMessage(nameof(UpdateContactPersonForm.Title))));

			form.AddMember(new("Jméno", nameof(UpdateContactPersonForm.Name), FormValueTypes.Text, person.Name, true,
				errors?.GetErrorMessage(nameof(UpdateContactPersonForm.Name))));

			form.AddMember(new("Příjmení", nameof(UpdateContactPersonForm.Surname), FormValueTypes.Text, person.Surname, true,
				errors?.GetErrorMessage(nameof(UpdateContactPersonForm.Surname))));

			form.AddMember(new("Obrázek", nameof(UpdateContactPersonForm.Base64Image), FormValueTypes.Text, person.Base64Image, true,
				errors?.GetErrorMessage(nameof(UpdateContactPersonForm.Base64Image))));

			form.AddMember(new("Role", nameof(UpdateContactPersonForm.Roles), FormValueTypes.TextArray, person.Roles, true,
				errors?.GetErrorMessage(nameof(UpdateContactPersonForm.Roles))));

			form.AddMember(new("Id", nameof(UpdateContactPersonForm.Id), FormValueTypes.Text, person.Id, false,
				errors?.GetErrorMessage(nameof(UpdateContactPersonForm.Id))));

			form.AddMember(new("Order", nameof(UpdateContactPersonForm.Order), FormValueTypes.Number, person.Order, true,
				errors?.GetErrorMessage(nameof(UpdateContactPersonForm.Order))));

			return form;
		}

		public static Form GetRemoveContactPersonForm(Guid personId, IEnumerable<ValidationFailure>? errors = null)
		{
			var form = new Form();
			form.AddMember(new("Id", nameof(RemoveContactPersonForm.Id), FormValueTypes.Text, personId, false,
				errors?.GetErrorMessage(nameof(RemoveContactPersonForm.Id))));

			return form;
		}
	}
}
