using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ODF.AppLayer.CQRS.Contact.Commands;
using ODF.AppLayer.Dtos.Validation;
using ODF.AppLayer.Mapping;
using ODF.AppLayer.Repos;

namespace ODF.AppLayer.CQRS.Contact.CommandHandlers
{
	internal class AddContactPersonCommandHandler : IRequestHandler<AddContactPersonCommand, ValidationDto>
	{
		private readonly IContactRepo _contactRepo;

		public AddContactPersonCommandHandler(IContactRepo contactRepo)
		{
			_contactRepo = contactRepo ?? throw new NotImplementedException(nameof(contactRepo));
		}

		public async Task<ValidationDto> Handle(AddContactPersonCommand request, CancellationToken cancellationToken)
			=> new() { IsOk = await _contactRepo.AddContactPersonAsync(request.MapToEntity(), cancellationToken) };
	}
}
