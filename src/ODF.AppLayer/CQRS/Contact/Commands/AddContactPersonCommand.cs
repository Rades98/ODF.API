using System.Collections.Generic;
using MediatR;
using ODF.AppLayer.Dtos.Validation;

namespace ODF.AppLayer.CQRS.Contact.Commands
{
	public class AddContactPersonCommand : IRequest<ValidationDto>
	{
		public AddContactPersonCommand(string email, string title, string name, string surname, IEnumerable<string> roles, string base64Image)
		{
			Email = email;
			Title = title;
			Name = name;
			Roles = roles;
			Base64Image = base64Image;
			Surname = surname;
		}

		public string Email { get; }

		public string Title { get; }

		public string Name { get; }

		public string Surname { get; }

		public IEnumerable<string> Roles { get; }

		public string Base64Image { get; }
	}
}
