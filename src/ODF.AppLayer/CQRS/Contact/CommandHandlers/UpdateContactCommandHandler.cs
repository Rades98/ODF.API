using System;
using System.Threading;
using System.Threading.Tasks;
using ODF.AppLayer.CQRS.Contact.Commands;
using ODF.AppLayer.Dtos.Validation;
using ODF.AppLayer.Mediator;
using ODF.AppLayer.Repos;

namespace ODF.AppLayer.CQRS.Contact.CommandHandlers
{
	internal class UpdateContactCommandHandler : ICommandHandler<UpdateContactCommand, ValidationDto>
	{
		private readonly IContactRepo _contactRepo;

		public UpdateContactCommandHandler(IContactRepo contactRepo)
		{
			_contactRepo = contactRepo ?? throw new NotImplementedException(nameof(contactRepo));
		}

		public async Task<ValidationDto> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
			=> new() { IsOk = await _contactRepo.UpdateContactAsync(request.EventName, request.EventManager, request.Email, cancellationToken) };
	}
}
