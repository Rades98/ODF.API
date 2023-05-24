using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ODF.AppLayer.CQRS.Contact.Commands;
using ODF.Data.Contracts.Interfaces;

namespace ODF.AppLayer.CQRS.Contact.CommandHandlers
{
	internal class RemoveBankAccountCommandHandler : IRequestHandler<RemoveBankAccountCommand, bool>
	{
		private readonly IContactRepo _contactRepo;

		public RemoveBankAccountCommandHandler(IContactRepo contactRepo)
		{
			_contactRepo = contactRepo ?? throw new NotImplementedException(nameof(contactRepo));
		}

		public Task<bool> Handle(RemoveBankAccountCommand request, CancellationToken cancellationToken)
			=> _contactRepo.RemoveBankAccountAsync(request.IBAN, cancellationToken);
	}
}
