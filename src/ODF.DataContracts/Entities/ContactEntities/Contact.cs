using System.Collections.Generic;

namespace ODF.Data.Contracts.Entities.ContactEntities
{
	public class Contact
	{
		public int Id = 1;

		public string EventName { get; set; }

		public string EventManager { get; set; }

		public string Email { get; set; }

		public Address Address { get; set; }

		public IEnumerable<BankAccount> BankAccounts { get; set; }

		public IEnumerable<ContactPerson> ContactPersons { get; set; }
	}
}
