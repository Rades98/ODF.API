using System;
using System.Threading;
using System.Threading.Tasks;
using ODF.AppLayer.CQRS.Contact.Commands;
using ODF.AppLayer.Dtos.Validation;
using ODF.AppLayer.Mediator;
using ODF.AppLayer.Repos;

namespace ODF.AppLayer.CQRS.Contact.CommandHandlers
{
	internal class UpdateContactAddressCommandHandler : ICommandHandler<UpdateContactAddressCommand, ValidationDto>
	{
		private readonly IContactRepo _contactRepo;

		public UpdateContactAddressCommandHandler(IContactRepo contactRepo)
		{
			_contactRepo = contactRepo ?? throw new NotImplementedException(nameof(contactRepo));
		}

		public async Task<ValidationDto> Handle(UpdateContactAddressCommand request, CancellationToken cancellationToken)
			=> new() { IsOk = await _contactRepo.UpdateAddressAsync(request.Street, request.City, request.PostalCode, request.Country, cancellationToken) };
	}
}
