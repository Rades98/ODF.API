using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ODF.AppLayer.CQRS.Contact.Commands;
using ODF.Data.Contracts.Interfaces;

namespace ODF.AppLayer.CQRS.Contact.CommandHandlers
{
	internal class UpdateContactCommandHandler : IRequestHandler<UpdateContactCommand, bool>
	{
		private readonly IContactRepo _contactRepo;

		public UpdateContactCommandHandler(IContactRepo contactRepo)
		{
			_contactRepo = contactRepo ?? throw new NotImplementedException(nameof(contactRepo));
		}

		public Task<bool> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
			=> _contactRepo.UpdateContactAsync(request.EventName, request.EventManager, request.Email, cancellationToken);
	}
}
