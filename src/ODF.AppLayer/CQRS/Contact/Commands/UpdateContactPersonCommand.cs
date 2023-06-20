using System;
using System.Collections.Generic;
using ODF.AppLayer.Dtos.Validation;
using ODF.AppLayer.Mediator;

namespace ODF.AppLayer.CQRS.Contact.Commands
{
	public class UpdateContactPersonCommand : ICommand<ValidationDto>
	{
		public UpdateContactPersonCommand(string email, string title, string name, string surname, IEnumerable<string> roles, string base64Image, Guid id, int order)
		{
			Email = email;
			Title = title;
			Name = name;
			Surname = surname;
			Roles = roles;
			Base64Image = base64Image;
			Id = id;
			Order = order;
		}

		public string Email { get; }

		public string Title { get; }

		public string Name { get; }

		public string Surname { get; }

		public IEnumerable<string> Roles { get; }

		public string Base64Image { get; }

		public Guid Id { get; }

		public int Order { get; }
	}
}
