namespace ODF.AppLayer.CQRS.Interfaces.Contact
{
	public interface IAddBankAccount
	{
		string Bank { get; }

		string AccountId { get; }

		string IBAN { get; }
	}
}
