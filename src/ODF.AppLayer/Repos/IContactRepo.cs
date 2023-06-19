using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ODF.Domain.Entities.ContactEntities;

namespace ODF.AppLayer.Repos
{
	public interface IContactRepo
	{
		Task<Contact> GetAsync(CancellationToken cancellationToken);

		Task<bool> UpdateContactAsync(string eventName, string eventManager, string email, CancellationToken cancellationToken);

		Task<bool> UpdateAddressAsync(string street, string city, string postalCode, string country, CancellationToken cancellationToken);

		Task<bool> AddBankAccountAsync(string bank, string accountId, string iban, CancellationToken cancellationToken);

		Task<bool> RemoveBankAccountAsync(string iban, CancellationToken cancellationToken);

		Task<bool> UpdateContactPersonAsync(ContactPerson person, CancellationToken cancellationToken);

		Task<bool> AddContactPersonAsync(ContactPerson person, CancellationToken cancellationToken);

		Task<bool> RemoveContactPersonAsync(Guid id, CancellationToken cancellationToken);

		Task<IEnumerable<BankAccount>> GetBankAccountsAsync(CancellationToken cancellationToken);
	}
}
