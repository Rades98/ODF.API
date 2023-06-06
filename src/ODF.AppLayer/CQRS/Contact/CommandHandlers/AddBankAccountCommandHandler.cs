using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ODF.AppLayer.CQRS.Contact.Commands;
using ODF.AppLayer.Dtos.Validation;
using ODF.AppLayer.Repos;

namespace ODF.AppLayer.CQRS.Contact.CommandHandlers
{
	internal class AddBankAccountCommandHandler : IRequestHandler<AddBankAccountCommand, ValidationDto>
	{
		private readonly IContactRepo _contactRepo;

		public AddBankAccountCommandHandler(IContactRepo contactRepo)
		{
			_contactRepo = contactRepo ?? throw new NotImplementedException(nameof(contactRepo));
		}

		public async Task<ValidationDto> Handle(AddBankAccountCommand request, CancellationToken cancellationToken)
			=> new() { IsOk = await _contactRepo.AddBankAccountAsync(request.Bank, request.AccountId, request.IBAN, cancellationToken) };
	}
}
