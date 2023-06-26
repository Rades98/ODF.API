using ODF.AppLayer.CQRS.Interfaces.Contact;
using ODF.AppLayer.Dtos.Validation;
using ODF.AppLayer.Mediator;

namespace ODF.AppLayer.CQRS.Contact.Commands
{
	public sealed class RemoveBankAccountCommand : ICommand<ValidationDto>, IRemoveBankAccount
	{
		public RemoveBankAccountCommand(string iban)
		{
			IBAN = iban;
		}

		public string IBAN { get; }
	}
}
