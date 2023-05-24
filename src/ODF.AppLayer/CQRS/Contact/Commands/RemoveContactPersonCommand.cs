using System;
using MediatR;

namespace ODF.AppLayer.CQRS.Contact.Commands
{
	public class RemoveContactPersonCommand : IRequest<bool>
	{
		public RemoveContactPersonCommand(Guid id)
		{
			Id = id;
		}

		public Guid Id { get; }
	}
}
