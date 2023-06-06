using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using ODF.AppLayer.CQRS.Contact.Commands;
using ODF.AppLayer.Repos;

namespace ODF.AppLayer.CQRS.Contact.CommandValidators
{
	public class AddBankAccountCommandValidator : AbstractValidator<AddBankAccountCommand>
	{
		private readonly IContactRepo _contactRepo;
		private static readonly Regex CzIBAN = new(@"^(?:CZ|cz)(?:\d{22}|\d{2}[ ]{1}(?:\d{4}[ ]){4}\d{4})$", RegexOptions.Compiled);

		public AddBankAccountCommandValidator(IContactRepo contactRepo)
		{
			_contactRepo = contactRepo ?? throw new ArgumentNullException(nameof(contactRepo));
		}

		public override async Task<ValidationResult> ValidateAsync(ValidationContext<AddBankAccountCommand> context, CancellationToken cancellationToken)
		{
			var contact = await _contactRepo.GetAsync(cancellationToken);

			var ibans = contact.BankAccounts.Select(ba => ba.IBAN);
			var accouuntIds = contact.BankAccounts.Select(ba => ba.AccountId);

			RuleFor(command => command.IBAN)
				.Must(CzIBAN.IsMatch)
				.WithMessage("Zadaný IBAN nesplňuje podmínky CZXXXXXXXXXXXXXXXXXXXXXX, případně czXX XXXX XXXX XXXX XXXX XXXX");

			RuleFor(command => command.IBAN)
				.Must(iban => !ibans.Contains(iban))
				.WithMessage("Zadaný IBAN už je použit");

			RuleFor(command => command.AccountId)
				.NotNull()
				.NotEmpty()
				.WithMessage("Číslo účtu musí být vyplněno");

			RuleFor(command => command.AccountId)
				.Must(iban => !ibans.Contains(iban))
				.WithMessage("Zadané bankovní číslo už je použito");

			RuleFor(command => command.Bank)
				.NotNull()
				.NotEmpty()
				.WithMessage("Banka musí být vyplněna");

			return await base.ValidateAsync(context, cancellationToken);
		}
	}
}
