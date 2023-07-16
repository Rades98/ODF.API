using ODF.AppLayer.CQRS.Interfaces.User;
using ODF.AppLayer.Dtos.Validation;
using ODF.AppLayer.Mediator;

namespace ODF.AppLayer.CQRS.User.Commands
{
	public class RegisterUserCommand : ICommand<ValidationDto>, IRegisterUser
	{
		public RegisterUserCommand(IRegisterUser input, string actiavtionLinkTemplate, string countryCode)
		{
			UserName = input.UserName;
			Password = input.Password;
			Password2 = input.Password2;
			Email = input.Email;
			FirstName = input.FirstName;
			LastName = input.LastName;
			CountryCode = countryCode;
			ActiavtionLinkTemplate = actiavtionLinkTemplate;
		}

		public string UserName { get; }

		public string Password { get; }

		public string Password2 { get; }

		public string Email { get; }

		public string FirstName { get; }

		public string LastName { get; }

		public string CountryCode { get; }

		public string ActiavtionLinkTemplate { get; set; }
	}
}
