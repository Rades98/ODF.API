using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ODF.AppLayer.CQRS.Contact.Commands;
using ODF.AppLayer.Mapping;
using ODF.Data.Contracts.Interfaces;

namespace ODF.AppLayer.CQRS.Contact.CommandHandlers
{
	internal class UpdateContactPersonCommandHandler : IRequestHandler<UpdateContactPersonCommand, bool>
	{
		private readonly IContactRepo _contactRepo;

		public UpdateContactPersonCommandHandler(IContactRepo contactRepo)
		{
			_contactRepo = contactRepo ?? throw new NotImplementedException(nameof(contactRepo));
		}

		public Task<bool> Handle(UpdateContactPersonCommand request, CancellationToken cancellationToken)
			=> _contactRepo.UpdateContactPersonAsync(request.MapToEntity(), cancellationToken);
	}
}
