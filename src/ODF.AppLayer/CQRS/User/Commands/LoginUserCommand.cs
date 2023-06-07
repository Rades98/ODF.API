using ODF.AppLayer.Dtos.User;
using ODF.AppLayer.Mediator;

namespace ODF.AppLayer.CQRS.User.Commands
{
	public class LoginUserCommand : ICommand<UserValidationDto>
	{
		public LoginUserCommand(string userName, string password)
		{
			UserName = userName;
			Password = password;
		}

		public string UserName { get; }

		public string Password { get; }
	}
}
