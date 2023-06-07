using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using ODF.AppLayer.CQRS.User.Commands;
using ODF.AppLayer.Repos;
using ODF.AppLayer.Services.Interfaces;

namespace ODF.AppLayer.CQRS.User.CommandValidators
{
	public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
	{
		private readonly IUserRepo _userRepo;
		private readonly IPasswordHasher _pwHahser;

		public LoginUserCommandValidator(IUserRepo userRepo, IPasswordHasher pwHahser)
		{
			_userRepo = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
			_pwHahser = pwHahser ?? throw new ArgumentNullException(nameof(pwHahser));
		}

		//TODO add middleware for rate limit login action
		public override async Task<ValidationResult> ValidateAsync(ValidationContext<LoginUserCommand> context, CancellationToken cancellationToken)
		{

			var user = await _userRepo.GetUserAsync(context.InstanceToValidate.UserName, cancellationToken);
			var pwValid = _pwHahser.Check(user.PasswordHash, context.InstanceToValidate.Password).Verified;

			RuleFor(command => command.Password)
				.Must(x => pwValid)
				.WithMessage("error_login_wrong_pw");

			return await base.ValidateAsync(context, cancellationToken);
		}
	}
}
