using System;
using ODF.AppLayer.CQRS.Interfaces.Contact;
using ODF.AppLayer.Dtos.Validation;
using ODF.AppLayer.Mediator;

namespace ODF.AppLayer.CQRS.Contact.Commands
{
	public sealed class RemoveContactPersonCommand : ICommand<ValidationDto>, IRemoveContactPerson
	{
		public RemoveContactPersonCommand(Guid id)
		{
			Id = id;
		}

		public Guid Id { get; }
	}
}
