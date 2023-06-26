using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using ODF.AppLayer.CQRS.Contact.Commands;
using ODF.AppLayer.Repos;
using ODF.Domain.Extensions;

namespace ODF.AppLayer.CQRS.Contact.CommandValidators
{
	public class AddBankAccountCommandValidator : AbstractValidator<AddBankAccountCommand>
	{
		private readonly IContactRepo _contactRepo;
		public AddBankAccountCommandValidator(IContactRepo contactRepo)
		{
			_contactRepo = contactRepo ?? throw new ArgumentNullException(nameof(contactRepo));
		}

		public override async Task<ValidationResult> ValidateAsync(ValidationContext<AddBankAccountCommand> context, CancellationToken cancellation = default)
		{
			var contact = await _contactRepo.GetAsync(cancellation);

			var ibans = contact.BankAccounts.Select(ba => ba.IBAN);
			var accouuntIds = contact.BankAccounts.Select(ba => ba.AccountId);

			RuleFor(command => command.IBAN)
				.Must(StringExtensions.ValidateIban)
				.WithMessage("Zadaný IBAN nesplňuje podmínky CZXXXXXXXXXXXXXXXXXXXXXX, případně czXX XXXX XXXX XXXX XXXX XXXX");

			RuleFor(command => command.IBAN)
				.Must(iban => !ibans.Contains(iban))
				.WithMessage("Zadaný IBAN už je použit");

			RuleFor(command => command.AccountId)
				.NotNull()
				.NotEmpty()
				.WithMessage("Číslo účtu musí být vyplněno");

			RuleFor(command => command.AccountId)
				.Must(accountId => !accouuntIds.Contains(accountId))
				.WithMessage("Zadané bankovní číslo už je použito");

			RuleFor(command => command.Bank)
				.NotNull()
				.NotEmpty()
				.WithMessage("Banka musí být vyplněna");

			return await base.ValidateAsync(context, cancellation);
		}
	}
}
