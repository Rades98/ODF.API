using ODF.AppLayer.Mediator;

namespace ODF.AppLayer.CQRS.Contact.Commands
{
	public class RemoveBankAccountCommand : ICommand<bool>
	{
		public RemoveBankAccountCommand(string iban)
		{
			IBAN = iban;
		}

		public string IBAN { get; }
	}
}
