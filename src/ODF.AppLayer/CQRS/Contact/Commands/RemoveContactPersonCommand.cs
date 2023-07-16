using System;
using ODF.AppLayer.CQRS.Interfaces.Contact;
using ODF.AppLayer.Dtos.Validation;
using ODF.AppLayer.Mediator;

namespace ODF.AppLayer.CQRS.Contact.Commands
{
	public sealed class RemoveContactPersonCommand : ICommand<ValidationDto>, IRemoveContactPerson
	{
		public RemoveContactPersonCommand(IRemoveContactPerson input)
		{
			Id = input.Id;
		}

		public Guid Id { get; }
	}
}
