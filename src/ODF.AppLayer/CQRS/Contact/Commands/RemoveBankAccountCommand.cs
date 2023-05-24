using MediatR;

namespace ODF.AppLayer.CQRS.Contact.Commands
{
	public class RemoveBankAccountCommand : IRequest<bool>
	{
		public RemoveBankAccountCommand(string iban)
		{
			IBAN = iban;
		}

		public string IBAN { get; }
	}
}
