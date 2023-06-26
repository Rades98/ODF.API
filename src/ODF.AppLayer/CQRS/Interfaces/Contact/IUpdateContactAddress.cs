namespace ODF.AppLayer.CQRS.Interfaces.Contact
{
	public interface IUpdateContactAddress
	{
		string Street { get; }

		string City { get; }

		string PostalCode { get; }

		string Country { get; }
	}
}
