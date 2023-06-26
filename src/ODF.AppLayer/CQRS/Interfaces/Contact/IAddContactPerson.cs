using System.Collections.Generic;

namespace ODF.AppLayer.CQRS.Interfaces.Contact
{
	public interface IAddContactPerson
	{
		string Email { get; }

		string Title { get; }

		string Name { get; }

		string Surname { get; }

		IEnumerable<string> Roles { get; }

		string Base64Image { get; }
	}
}
