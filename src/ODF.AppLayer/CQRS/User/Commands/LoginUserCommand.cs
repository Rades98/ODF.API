using ODF.AppLayer.CQRS.Interfaces.User;
using ODF.AppLayer.Dtos.User;
using ODF.AppLayer.Mediator;

namespace ODF.AppLayer.CQRS.User.Commands
{
	public sealed class LoginUserCommand : ICommand<UserValidationDto>, ILoginUser
	{
		public LoginUserCommand(ILoginUser input, string countryCode)
		{
			UserName = input.UserName;
			Password = input.Password;
			CountryCode = countryCode;
		}

		public string UserName { get; }

		public string Password { get; }

		public string CountryCode { get; }
	}
}
