using MediatR;
using ODF.AppLayer.Dtos.User;

namespace ODF.AppLayer.CQRS.User.Commands
{
	public class LoginUserCommand : IRequest<UserValidationDto>
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
