using System;
using System.Collections.Generic;
using System.Linq;

namespace ODF.Domain.Entities.ContactEntities
{
	public class ContactPerson
	{
		public string Email { get; set; } = string.Empty;

		public string Title { get; set; } = string.Empty;

		public string Name { get; set; } = string.Empty;

		public string Surname { get; set; } = string.Empty;

		public IEnumerable<string> Roles { get; set; } = Enumerable.Empty<string>();

		public string Base64Image { get; set; } = string.Empty;

		public int? Order { get; set; }

		public Guid Id { get; set; }
	}
}
