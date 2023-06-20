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
	public class RemoveContactPersonCommandValidator : AbstractValidator<RemoveContactPersonCommand>
	{
		private readonly IContactRepo _contactRepo;

		public RemoveContactPersonCommandValidator(IContactRepo contactRepo)
		{
			_contactRepo = contactRepo ?? throw new ArgumentNullException(nameof(contactRepo));
		}

		public override async Task<ValidationResult> ValidateAsync(ValidationContext<RemoveContactPersonCommand> context, CancellationToken cancellationToken)
		{
			var contact = await _contactRepo.GetAsync(cancellationToken);
			bool exist = contact.ContactPersons.Any(person => person.Id == context.InstanceToValidate.Id);

			RuleFor(command => command.Id)
				.Must(command => exist)
				.WithMessage("Nebyla nalezena taková kontaktní osoba");

			return await base.ValidateAsync(context, cancellationToken);
		}
	}
}
