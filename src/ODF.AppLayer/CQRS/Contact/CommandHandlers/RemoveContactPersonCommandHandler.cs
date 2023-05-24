using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ODF.AppLayer.CQRS.Contact.Commands;
using ODF.Data.Contracts.Interfaces;

namespace ODF.AppLayer.CQRS.Contact.CommandHandlers
{
	internal class RemoveContactPersonCommandHandler : IRequestHandler<RemoveContactPersonCommand, bool>
	{
		private readonly IContactRepo _contactRepo;

		public RemoveContactPersonCommandHandler(IContactRepo contactRepo)
		{
			_contactRepo = contactRepo ?? throw new NotImplementedException(nameof(contactRepo));
		}

		public Task<bool> Handle(RemoveContactPersonCommand request, CancellationToken cancellationToken)
			=> _contactRepo.RemoveContactPersonAsync(request.Id, cancellationToken);
	}
}
