using System.Collections.Generic;

namespace ODF.AppLayer.Dtos.ContactDtos
{
	public class ContactDto
	{
		public string EventName { get; set; }

		public string EventManager { get; set; }

		public string EventManagerTranslation { get; set; }

		public string Email { get; set; }

		public string EmailTranslation { get; set; }

		public AddressDto Address { get; set; }

		public IEnumerable<BankAccountDto> BankAccounts { get; set; }

		public IEnumerable<ContactPersonDto> ContactPersons { get; set; }
	}
}
