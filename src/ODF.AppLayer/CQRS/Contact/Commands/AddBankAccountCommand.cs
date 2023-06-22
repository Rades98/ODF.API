using ODF.AppLayer.Dtos.Validation;
using ODF.AppLayer.Mediator;

namespace ODF.AppLayer.CQRS.Contact.Commands
{
	public sealed class AddBankAccountCommand : ICommand<ValidationDto>
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
