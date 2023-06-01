using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using ODF.AppLayer.CQRS.User.Commands;
using ODF.AppLayer.PWHashing;
using ODF.Data.Contracts.Interfaces;

namespace ODF.AppLayer.CQRS.User.CommandValidators
{
	internal class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
	{
		private readonly IUserRepo _userRepo;

		public LoginUserCommandValidator(IUserRepo userRepo)
		{
			_userRepo = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
		}

		public override async Task<ValidationResult> ValidateAsync(ValidationContext<LoginUserCommand> context, CancellationToken cancellationToken)
		{
			var user = await _userRepo.GetUserAsync(context.InstanceToValidate.UserName, cancellationToken);
			byte[] hash = Encoding.UTF8.GetBytes(user.PasswordHash);
			byte[] salt = Encoding.UTF8.GetBytes(user.PasswordSalt);

			RuleFor(command => command.Password)
				.Must(x => PasswordUtils.VerifyPasswordHash(context.InstanceToValidate.Password, hash, salt))
				.WithMessage("More spatne heslo");

			return await base.ValidateAsync(context, cancellationToken);
		}
	}
}
