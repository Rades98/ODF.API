namespace ODF.AppLayer.CQRS.Interfaces.User
{
	public interface ILoginUser
	{
		string UserName { get; }

		string Password { get; }
	}
}
