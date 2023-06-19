using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ODF.AppLayer.CQRS.Contact.Queries;
using ODF.AppLayer.Dtos.ContactDtos;
using ODF.AppLayer.Mediator;
using ODF.AppLayer.Repos;

namespace ODF.AppLayer.CQRS.Contact.QueryHandlers
{
	internal class GetBankAccountsQRQueryHandler : IQueryHandler<GetBankAccountsQRQuery, IEnumerable<BankAccountQRDto>>
	{
		private readonly IContactRepo _contactRepo;

		public GetBankAccountsQRQueryHandler(IContactRepo contactRepo)
		{
			_contactRepo = contactRepo ?? throw new ArgumentNullException(nameof(contactRepo));
		}

		public async Task<IEnumerable<BankAccountQRDto>> Handle(GetBankAccountsQRQuery request, CancellationToken cancellationToken)
		{
			var accs = await _contactRepo.GetBankAccountsAsync(cancellationToken);

			return accs.Select(acc => new BankAccountQRDto() { IBAN = acc.IBAN });
		}
	}
}
