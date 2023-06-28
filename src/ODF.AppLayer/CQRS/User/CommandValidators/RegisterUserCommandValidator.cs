using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using ODF.AppLayer.CQRS.User.Commands;
using ODF.AppLayer.Extensions;
using ODF.AppLayer.Repos;
using ODF.AppLayer.Services.Interfaces;
using ODF.Domain.Extensions;

namespace ODF.AppLayer.CQRS.User.CommandValidators
{
	public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
	{
		private readonly IUserRepo _userRepo;
		private readonly ITranslationsProvider _translationsProvider;

		public RegisterUserCommandValidator(IUserRepo userRepo, ITranslationsProvider translationsProvider)
		{
			_userRepo = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
			_translationsProvider = translationsProvider ?? throw new ArgumentNullException(nameof(translationsProvider));
		}

		public override async Task<ValidationResult> ValidateAsync(ValidationContext<RegisterUserCommand> context, CancellationToken cancellation = default)
		{
			var transaltions = await _translationsProvider.GetTranslationsAsync(context.InstanceToValidate.CountryCode, cancellation);

			var userNames = await _userRepo.GetUserNamesAsync(cancellation);
			var userEmails = await _userRepo.GetUserEmailsAsync(cancellation);

			RuleFor(command => command.UserName)
				.Must(name => !userNames.Contains(name))
				.WithMessage(transaltions.Get("error_register_name_in_use"));

			RuleFor(command => command.UserName)
				.Must(name => !name.Contains(" "))
				.WithMessage(transaltions.Get("error_register_name_whitespace"));

			RuleFor(command => command.Email)
				.Must(email => !userEmails.Contains(email))
				.WithMessage(transaltions.Get("error_register_email_in_use"));

			RuleFor(command => command.Email)
				.Must(StringExtensions.ValidateEmail)
				.WithMessage(transaltions.Get("error_register_email_invalid"));

			RuleFor(command => command.Password)
				.Must(password => password == context.InstanceToValidate.Password2)
				.WithMessage(transaltions.Get("error_register_pw_missmatch"));

			RuleFor(command => command.Password2)
				.Must(password => password == context.InstanceToValidate.Password)
				.WithMessage(transaltions.Get("error_register_pw_missmatch"));

			RuleFor(command => command.Password)
				.Must(StringExtensions.ValidatePassword)
				.WithMessage(transaltions.Get("error_register_pw_invalid"));

			RuleFor(command => command.Password2)
				.Must(StringExtensions.ValidatePassword)
				.WithMessage(transaltions.Get("error_register_pw_invalid"));

			RuleFor(command => command.FirstName)
				.Must(firstName => firstName is not null)
				.WithMessage(transaltions.Get("error_register_name_invalid"));

			return await base.ValidateAsync(context, cancellation);
		}
	}
}
