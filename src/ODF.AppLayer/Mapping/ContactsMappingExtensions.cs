using ODF.AppLayer.CQRS.Contact.Commands;
using ODF.AppLayer.Dtos.ContactDtos;
using ODF.Domain.Entities.ContactEntities;

namespace ODF.AppLayer.Mapping
{
	internal static class ContactsMappingExtensions
	{
		public static AddressDto MapAddress(this Address address)
			=> new()
			{
				City = address.City,
				PostalCode = address.PostalCode,
				Country = address.Country,
				Street = address.Street,
			};

		public static BankAccountDto MapBankAccount(this BankAccount bankAccount, string accountIdTranslation, string bankTranslation, string ibanTranslation)
			=> new()
			{
				AccountId = bankAccount.AccountId,
				Bank = bankAccount.Bank,
				IBAN = bankAccount.IBAN,
				AccountIdTranslation = accountIdTranslation,
				BankTranslation = bankTranslation,
				IBANTranslation = ibanTranslation,
			};

		public static ContactPersonDto MapContactPerson(this ContactPerson cp)
			=> new()
			{
				Base64Image = cp.Base64Image,
				Email = cp.Email,
				Name = cp.Name,
				Roles = cp.Roles,
				Surname = cp.Surname,
				Title = cp.Title,
				Id = cp.Id,
				Order = cp.Order.Value,
			};

		public static ContactPerson MapToEntity(this UpdateContactPersonCommand command)
			=> new()
			{
				Id = command.Id,
				Base64Image = command.Base64Image,
				Email = command.Email,
				Name = command.Name,
				Roles = command.Roles,
				Surname = command.Surname,
				Title = command.Title,
				Order = command.Order
			};

		public static ContactPerson MapToEntity(this AddContactPersonCommand command)
			=> new()
			{
				Base64Image = command.Base64Image,
				Email = command.Email,
				Name = command.Name,
				Roles = command.Roles,
				Surname = command.Surname,
				Title = command.Title,
			};
	}
}
