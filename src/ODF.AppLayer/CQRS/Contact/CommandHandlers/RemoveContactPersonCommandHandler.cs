using System;
using System.Threading;
using System.Threading.Tasks;
using ODF.AppLayer.CQRS.Contact.Commands;
using ODF.AppLayer.Mediator;
using ODF.AppLayer.Repos;

namespace ODF.AppLayer.CQRS.Contact.CommandHandlers
{
	internal class RemoveContactPersonCommandHandler : ICommandHandler<RemoveContactPersonCommand, bool>
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
