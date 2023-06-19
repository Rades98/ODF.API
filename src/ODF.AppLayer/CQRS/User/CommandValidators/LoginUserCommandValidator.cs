using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using ODF.AppLayer.CQRS.User.Commands;
using ODF.AppLayer.Repos;
using ODF.Domain.Utils;

namespace ODF.AppLayer.CQRS.User.CommandValidators
{
	public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
	{
		private readonly IUserRepo _userRepo;

		public LoginUserCommandValidator(IUserRepo userRepo)
		{
			_userRepo = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
		}

		//TODO add middleware for rate limit login action
		public override async Task<ValidationResult> ValidateAsync(ValidationContext<LoginUserCommand> context, CancellationToken cancellationToken)
		{

			var user = await _userRepo.GetUserAsync(context.InstanceToValidate.UserName, cancellationToken);
			bool pwValid = PasswordHasher.Check(user.PasswordHash, context.InstanceToValidate.Password).Verified;

			RuleFor(command => command.Password)
				.Must(x => pwValid)
				.WithMessage("error_login_wrong_pw");

			return await base.ValidateAsync(context, cancellationToken);
		}
	}
}
