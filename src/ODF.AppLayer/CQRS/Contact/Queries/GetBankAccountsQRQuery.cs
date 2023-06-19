using System.Collections.Generic;
using ODF.AppLayer.Dtos.ContactDtos;
using ODF.AppLayer.Mediator;

namespace ODF.AppLayer.CQRS.Contact.Queries
{
	public class GetBankAccountsQRQuery : IQuery<IEnumerable<BankAccountQRDto>>
	{
		public GetBankAccountsQRQuery()
		{

		}
	}
}
