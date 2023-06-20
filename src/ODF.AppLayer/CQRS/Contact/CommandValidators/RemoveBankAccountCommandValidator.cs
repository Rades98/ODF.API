using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using ODF.AppLayer.CQRS.Contact.Commands;
using ODF.AppLayer.Repos;

namespace ODF.AppLayer.CQRS.Contact.CommandValidators
{
	public class RemoveBankAccountCommandValidator : AbstractValidator<RemoveBankAccountCommand>
	{
		private readonly IContactRepo _contactRepo;

		public RemoveBankAccountCommandValidator(IContactRepo contactRepo)
		{
			_contactRepo = contactRepo ?? throw new ArgumentNullException(nameof(contactRepo));
		}

		public override async Task<ValidationResult> ValidateAsync(ValidationContext<RemoveBankAccountCommand> context, CancellationToken cancellationToken)
		{
			var contact = await _contactRepo.GetAsync(cancellationToken);
			bool exist = contact.BankAccounts.Select(ba => ba.IBAN.ToUpper().Replace(" ", "")).Any(iban => iban == context.InstanceToValidate.IBAN);

			RuleFor(command => command.IBAN)
				.NotEmpty()
				.WithMessage("IBAN musí být vyplněn");

			RuleFor(command => command.IBAN)
				.Must(command => exist)
				.WithMessage("Nebyl nalezen takový účet");

			return await base.ValidateAsync(context, cancellationToken);
		}
	}
}
