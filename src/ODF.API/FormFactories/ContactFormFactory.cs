using FluentValidation.Results;
using ODF.API.RequestModels.Forms.Contacts;
using ODF.API.ResponseModels.Common.Forms;
using ODF.AppLayer.Dtos.ContactDtos;

namespace ODF.API.FormFactories
{
	public static class ContactFormFactory
	{
		public static Form GetUpdateContactForm(ContactDto contact)
		{
			var form = new Form();
			form.AddMember(new("Název akce", nameof(UpdateContactForm.EventName), "text", contact.EventName, true));
			form.AddMember(new("Pořadatel", nameof(UpdateContactForm.EventManager), "text", contact.EventManager, true));
			form.AddMember(new("e-mail", nameof(UpdateContactForm.Email), "text", contact.Email, true));

			return form;
		}

		public static Form GetUpdateAddressForm(AddressDto address)
		{
			var form = new Form();

			form.AddMember(new("Město", nameof(UpdateAddressForm.City), "text", address.City, true));
			form.AddMember(new("Ulice", nameof(UpdateAddressForm.Street), "text", address.Street, true));
			form.AddMember(new("PSČ", nameof(UpdateAddressForm.PostalCode), "text", address.PostalCode, true));
			form.AddMember(new("Země", nameof(UpdateAddressForm.Country), "text", address.Country, true));

			return form;
		}

		public static Form GetAddBankAcountForm(IEnumerable<ValidationFailure>? errors = null, string? bank = "", string accountId = "", string iban = "")
		{
			var form = new Form();
			form.AddMember(new("Banka", nameof(AddBankAccountForm.Bank), "text", bank, true, errors?.FirstOrDefault(p => p.PropertyName == nameof(AddBankAccountForm.Bank))?.ErrorMessage));
			form.AddMember(new("Číslo účtu", nameof(AddBankAccountForm.AccountId), "text", accountId, true, errors?.FirstOrDefault(p => p.PropertyName == nameof(AddBankAccountForm.AccountId))?.ErrorMessage));
			form.AddMember(new("IBAN", nameof(AddBankAccountForm.IBAN), "text", iban, true, errors?.FirstOrDefault(p => p.PropertyName == nameof(AddBankAccountForm.IBAN))?.ErrorMessage));

			return form;
		}

		public static Form GetRemoveBankAcountForm(string iban)
		{
			var form = new Form();
			form.AddMember(new("IBAN", nameof(AddBankAccountForm.IBAN), "text", iban, true));

			return form;
		}

		public static Form GetAddContactPersonForm(IEnumerable<ValidationFailure>? errors = null, AddContactPersonForm? reqForm = null)
		{
			var form = new Form();
			form.AddMember(new("e-mail", nameof(AddContactPersonForm.Email), "text", reqForm?.Email ?? "", true, errors?.FirstOrDefault(p => p.PropertyName == nameof(AddContactPersonForm.Email))?.ErrorMessage));
			form.AddMember(new("Titul", nameof(AddContactPersonForm.Title), "text", reqForm?.Title ?? "", true, errors?.FirstOrDefault(p => p.PropertyName == nameof(AddContactPersonForm.Title))?.ErrorMessage));
			form.AddMember(new("Jméno", nameof(AddContactPersonForm.Name), "text", reqForm?.Name ?? "", true, errors?.FirstOrDefault(p => p.PropertyName == nameof(AddContactPersonForm.Name))?.ErrorMessage));
			form.AddMember(new("Příjmení", nameof(AddContactPersonForm.Surname), reqForm?.Surname ?? "text", "", true, errors?.FirstOrDefault(p => p.PropertyName == nameof(AddContactPersonForm.Surname))?.ErrorMessage));
			form.AddMember(new("Obrázek", nameof(AddContactPersonForm.Base64Image), reqForm?.Base64Image ?? "text", "", true, errors?.FirstOrDefault(p => p.PropertyName == nameof(AddContactPersonForm.Base64Image))?.ErrorMessage));
			form.AddMember(new("Role", nameof(AddContactPersonForm.Roles), "text", reqForm?.Roles ?? Enumerable.Empty<string>(), true, errors?.FirstOrDefault(p => p.PropertyName == nameof(AddContactPersonForm.Roles))?.ErrorMessage));

			return form;
		}

		public static Form GetUpdateContactPersonForm(ContactPersonDto person)
		{
			var form = new Form();

			form.AddMember(new("e-mail", nameof(UpdateContactPersonForm.Email), "text", person.Email, true));
			form.AddMember(new("Titul", nameof(UpdateContactPersonForm.Title), "text", person.Title, true));
			form.AddMember(new("Jméno", nameof(UpdateContactPersonForm.Name), "text", person.Name, true));
			form.AddMember(new("Příjmení", nameof(UpdateContactPersonForm.Surname), "text", person.Surname, true));
			form.AddMember(new("Obrázek", nameof(UpdateContactPersonForm.Base64Image), "text", person.Base64Image, true));
			form.AddMember(new("Role", nameof(UpdateContactPersonForm.Roles), "textArray", person.Roles, true));
			form.AddMember(new("Id", nameof(UpdateContactPersonForm.Id), "text", person.Id));
			form.AddMember(new("Order", nameof(UpdateContactPersonForm.Order), "number", person.Order, true));

			return form;
		}

		public static Form GetRemoveContactPersonForm(Guid personId)
		{
			var form = new Form();
			form.AddMember(new("Id", nameof(RemoveContactPersonForm.Id), "text", personId));

			return form;
		}
	}
}
