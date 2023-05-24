using MediatR;
using ODF.AppLayer.Dtos.Validation;

namespace ODF.AppLayer.CQRS.Contact.Commands
{
	public class AddBankAccountCommand : IRequest<ValidationDto>
	{
		public AddBankAccountCommand(string bank, string accountId, string iban)
		{
			Bank = bank;
			AccountId = accountId;
			IBAN = iban;
		}

		public string Bank { get; }

		public string AccountId { get; }

		public string IBAN { get; }
	}
}
