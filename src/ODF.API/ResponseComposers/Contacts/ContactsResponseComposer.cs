using ODF.API.ResponseModels.Contacts.GetContacts;
using ODF.AppLayer.Dtos.ContactDtos;

namespace ODF.API.ResponseComposers.Contacts
{
	internal static class ContactsResponseComposer
	{
		internal static ContactResponseModel GetContactResponse(string countryCode, string baseUrl, ContactDto model)
		{
			var responseModel = new ContactResponseModel();

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
	}
}
