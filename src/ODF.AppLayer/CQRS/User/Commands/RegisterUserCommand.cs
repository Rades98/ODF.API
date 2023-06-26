using ODF.AppLayer.CQRS.Interfaces.User;
using ODF.AppLayer.Dtos.Validation;
using ODF.AppLayer.Mediator;

namespace ODF.AppLayer.CQRS.User.Commands
{
	public class RegisterUserCommand : ICommand<ValidationDto>, IRegisterUser
	{
		public RegisterUserCommand(string countryCode, string userName, string password, string password2, string email, string firstName, string actiavtionLinkTemplate, string lastName = null)
		{
			UserName = userName;
			Password = password;
			Password2 = password2;
			Email = email;
			FirstName = firstName;
			LastName = lastName;
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
