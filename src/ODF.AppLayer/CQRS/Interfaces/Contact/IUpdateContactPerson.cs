using System;
using System.Collections.Generic;

namespace ODF.AppLayer.CQRS.Interfaces.Contact
{
	public interface IUpdateContactPerson
	{
		string Email { get; }

		string Title { get; }

		string Name { get; }

		string Surname { get; }

		IEnumerable<string> Roles { get; }

		string Base64Image { get; }

		Guid Id { get; }

		int Order { get; }
	}
}
