using System;
using System.Collections.Generic;

namespace ODF.Domain.Entities.ContactEntities
{
	public class ContactPerson
	{
		public string Email { get; set; }

		public string Title { get; set; }

		public string Name { get; set; }

		public string Surname { get; set; }

		public IEnumerable<string> Roles { get; set; }

		public string Base64Image { get; set; }

		public int? Order { get; set; }

		public Guid Id { get; set; }
	}
}
