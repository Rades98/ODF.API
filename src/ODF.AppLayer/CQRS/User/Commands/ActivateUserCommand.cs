using ODF.AppLayer.CQRS.Interfaces.User;
using ODF.AppLayer.Dtos.Validation;
using ODF.AppLayer.Mediator;

namespace ODF.AppLayer.CQRS.User.Commands
{
	public class ActivateUserCommand : ICommand<ValidationDto>, IActivateUser
	{
		public ActivateUserCommand(IActivateUser input, string countryCode)
		{
			Hash = input.Hash;
			CountryCode = countryCode;
		}

		public string Hash { get; }

		public string CountryCode { get; }
	}
}
