using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using ODF.AppLayer.CQRS.User.Commands;
using ODF.AppLayer.PWHashing;
using ODF.AppLayer.Repos;
using ODF.AppLayer.Services.Interfaces;

namespace ODF.AppLayer.CQRS.User.CommandValidators
{
	public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
	{
		private readonly IUserRepo _userRepo;
		private readonly ITranslationsProvider _translationsProvider;

		public LoginUserCommandValidator(IUserRepo userRepo, ITranslationsProvider translationsProvider)
		{
			_userRepo = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
			_translationsProvider = translationsProvider ?? throw new ArgumentNullException(nameof(translationsProvider));
		}

		//TODO add middleware for rate limit login action
		public override async Task<ValidationResult> ValidateAsync(ValidationContext<LoginUserCommand> context, CancellationToken cancellationToken)
		{

			var user = await _userRepo.GetUserAsync(context.InstanceToValidate.UserName, cancellationToken);
			byte[] hash = Encoding.UTF8.GetBytes(user.PasswordHash);
			byte[] salt = Encoding.UTF8.GetBytes(user.PasswordSalt);

			RuleFor(command => command.Password)
				.Must(x => PasswordUtils.VerifyPasswordHash(context.InstanceToValidate.Password, hash, salt))
				.WithMessage("error_login_wrong_pw");

			return await base.ValidateAsync(context, cancellationToken);
		}
	}
}
