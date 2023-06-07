using System;
using ODF.AppLayer.Mediator;

namespace ODF.AppLayer.CQRS.Contact.Commands
{
	public class RemoveContactPersonCommand : ICommand<bool>
	{
		public RemoveContactPersonCommand(Guid id)
		{
			Id = id;
		}

		public Guid Id { get; }
	}
}
