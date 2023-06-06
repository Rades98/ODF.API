using System;
using System.Threading;
using System.Threading.Tasks;
using ODF.Domain.Entities.ContactEntities;

namespace ODF.AppLayer.Repos
{
	public interface IContactRepo
	{
		public Task<Contact> GetAsync(CancellationToken cancellationToken);

		public Task<bool> UpdateContactAsync(string eventName, string eventManager, string email, CancellationToken cancellationToken);

		public Task<bool> UpdateAddressAsync(string street, string city, string postalCode, string country, CancellationToken cancellationToken);

		public Task<bool> AddBankAccountAsync(string bank, string accountId, string iban, CancellationToken cancellationToken);

		public Task<bool> RemoveBankAccountAsync(string iban, CancellationToken cancellationToken);

		public Task<bool> UpdateContactPersonAsync(ContactPerson person, CancellationToken cancellationToken);

		public Task<bool> AddContactPersonAsync(ContactPerson person, CancellationToken cancellationToken);

		public Task<bool> RemoveContactPersonAsync(Guid id, CancellationToken cancellationToken);
	}
}
