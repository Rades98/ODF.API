using System;
using System.Collections.Generic;
using ODF.AppLayer.CQRS.Interfaces.Contact;
using ODF.AppLayer.Dtos.Validation;
using ODF.AppLayer.Mediator;

namespace ODF.AppLayer.CQRS.Contact.Commands
{
	public sealed class UpdateContactPersonCommand : ICommand<ValidationDto>, IUpdateContactPerson
	{
		public UpdateContactPersonCommand(IUpdateContactPerson input)
		{
			Email = input.Email;
			Title = input.Title;
			Name = input.Name;
			Surname = input.Surname;
			Roles = input.Roles;
			Base64Image = input.Base64Image;
			Id = input.Id;
			Order = input.Order;
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
