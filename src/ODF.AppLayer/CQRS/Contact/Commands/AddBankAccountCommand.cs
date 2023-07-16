using ODF.AppLayer.CQRS.Interfaces.Contact;
using ODF.AppLayer.Dtos.Validation;
using ODF.AppLayer.Mediator;

namespace ODF.AppLayer.CQRS.Contact.Commands
{
	public sealed class AddBankAccountCommand : ICommand<ValidationDto>, IAddBankAccount
	{
		public AddBankAccountCommand(IAddBankAccount input)
		{
			Bank = input.Bank;
			AccountId = input.AccountId;
			IBAN = input.IBAN;
		}

		public string Bank { get; }

		public string AccountId { get; }

		public string IBAN { get; }
	}
}
