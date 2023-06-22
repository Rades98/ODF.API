using System;
using ODF.AppLayer.Dtos.Validation;
using ODF.AppLayer.Mediator;

namespace ODF.AppLayer.CQRS.Contact.Commands
{
	public sealed class RemoveContactPersonCommand : ICommand<ValidationDto>
	{
		public RemoveContactPersonCommand(Guid id)
		{
			Id = id;
		}

		public Guid Id { get; }
	}
}
