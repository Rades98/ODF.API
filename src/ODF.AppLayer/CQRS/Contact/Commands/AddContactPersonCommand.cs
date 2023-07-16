using System.Collections.Generic;
using ODF.AppLayer.CQRS.Interfaces.Contact;
using ODF.AppLayer.Dtos.Validation;
using ODF.AppLayer.Mediator;

namespace ODF.AppLayer.CQRS.Contact.Commands
{
	public sealed class AddContactPersonCommand : ICommand<ValidationDto>, IAddContactPerson
	{
		public AddContactPersonCommand(IAddContactPerson input)
		{
			Email = input.Email;
			Title = input.Title;
			Name = input.Name;
			Roles = input.Roles;
			Base64Image = input.Base64Image;
			Surname = input.Surname;
		}

		public string Email { get; }

		public string Title { get; }

		public string Name { get; }

		public string Surname { get; }

		public IEnumerable<string> Roles { get; }

		public string Base64Image { get; }
	}
}
