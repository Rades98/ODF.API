using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using ODF.AppLayer.CQRS.User.Commands;
using ODF.AppLayer.Extensions;
using ODF.AppLayer.Repos;
using ODF.AppLayer.Services.Interfaces;

namespace ODF.AppLayer.CQRS.User.CommandValidators
{
	public class ActivateUserCommandValidator : AbstractValidator<ActivateUserCommand>
	{
		private readonly IUserRepo _userRepo;
		private readonly ITranslationsProvider _translationsProvider;

		public ActivateUserCommandValidator(IUserRepo userRepo, ITranslationsProvider translationsProvider)
		{
			_userRepo = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
			_translationsProvider = translationsProvider ?? throw new ArgumentNullException(nameof(translationsProvider));
		}

		public override async Task<ValidationResult> ValidateAsync(ValidationContext<ActivateUserCommand> context, CancellationToken cancellation = default)
		{
			var transaltions = await _translationsProvider.GetTranslationsAsync(context.InstanceToValidate.CountryCode, cancellation);

			var user = await _userRepo.GetUserByHashAsync(context.InstanceToValidate.Hash, cancellation);

			RuleFor(command => command.Hash)
					.Must(x => user is not null)
					.WithMessage(transaltions.Get("error_activate_wrong_hash"));

			if (user is not null)
			{
				RuleFor(command => command.Hash)
					.Must(x => user.ChangeHashValidTo > DateTime.Now)
					.WithMessage(transaltions.Get("error_activate_hash_too_old"));
			}

			return await base.ValidateAsync(context, cancellation);
		}
	}
}
