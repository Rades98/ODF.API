using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using ODF.AppLayer.CQRS.User.Commands;
using ODF.AppLayer.Extensions;
using ODF.AppLayer.Repos;
using ODF.AppLayer.Services.Interfaces;
using ODF.Domain.Utils;

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
		public override async Task<ValidationResult> ValidateAsync(ValidationContext<LoginUserCommand> context, CancellationToken cancellation = default)
		{
			var transaltions = await _translationsProvider.GetTranslationsAsync(context.InstanceToValidate.CountryCode, cancellation);

			var user = await _userRepo.GetUserAsync(context.InstanceToValidate.UserName, cancellation);

			RuleFor(command => command.UserName)
					.Must(x => user is not null)
					.WithMessage(transaltions.Get("error_login_wrong_user"));

			if (user is not null)
			{
				bool pwValid = PasswordHasher.Check(user.PasswordHash, context.InstanceToValidate.Password).Verified;

				RuleFor(command => command.Password)
					.Must(x => pwValid)
					.WithMessage(transaltions.Get("error_login_wrong_pw"));
			}



			return await base.ValidateAsync(context, cancellation);
		}
	}
}
