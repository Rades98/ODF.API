using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ODF.AppLayer.CQRS.Contact.Commands;
using ODF.AppLayer.Repos;

namespace ODF.AppLayer.CQRS.Contact.CommandHandlers
{
	internal class UpdateContactAddressCommandHandler : IRequestHandler<UpdateContactAddressCommand, bool>
	{
		private readonly IContactRepo _contactRepo;

		public UpdateContactAddressCommandHandler(IContactRepo contactRepo)
		{
			_contactRepo = contactRepo ?? throw new NotImplementedException(nameof(contactRepo));
		}

		public Task<bool> Handle(UpdateContactAddressCommand request, CancellationToken cancellationToken)
			=> _contactRepo.UpdateAddressAsync(request.Street, request.City, request.PostalCode, request.Country, cancellationToken);
	}
}
