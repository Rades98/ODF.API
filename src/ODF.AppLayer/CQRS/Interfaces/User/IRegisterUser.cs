namespace ODF.AppLayer.CQRS.Interfaces.User
{
	public interface IRegisterUser
	{
		string UserName { get; }

		string Password { get; }

		string Password2 { get; }

		string Email { get; }

		string FirstName { get; }

		string LastName { get; }
	}
}
