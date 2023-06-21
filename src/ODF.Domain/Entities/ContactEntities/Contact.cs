using System.Collections.Generic;

namespace ODF.Domain.Entities.ContactEntities
{
	public class Contact
	{
		public int Id { get; set; } = 1;

		public string EventName { get; set; } = string.Empty;

		public string EventManager { get; set; } = string.Empty;

		public string Email { get; set; } = string.Empty;

		public Address Address { get; set; } = new();

		public IEnumerable<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();

		public IEnumerable<ContactPerson> ContactPersons { get; set; } = new List<ContactPerson>();
	}
}
