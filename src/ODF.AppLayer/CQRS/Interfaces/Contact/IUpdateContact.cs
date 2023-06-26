namespace ODF.AppLayer.CQRS.Interfaces.Contact
{
	public interface IUpdateContact
	{
		string EventName { get; }

		string EventManager { get; }

		string Email { get; }
	}
}
