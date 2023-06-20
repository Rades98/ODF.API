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
	public class UpdateContactPersonCommandValidator : AbstractValidator<UpdateContactPersonCommand>
	{
		private readonly IContactRepo _contactRepo;

		public UpdateContactPersonCommandValidator(IContactRepo contactRepo)
		{
			_contactRepo = contactRepo ?? throw new ArgumentNullException(nameof(contactRepo));
		}

		public override async Task<ValidationResult> ValidateAsync(ValidationContext<UpdateContactPersonCommand> context, CancellationToken cancellationToken)
		{
			var contact = await _contactRepo.GetAsync(cancellationToken);
			bool exist = contact.ContactPersons.Any(person => person.Id == context.InstanceToValidate.Id);

			RuleFor(person => person.Id)
				.Must(x => exist)
				.WithMessage("Kontaktní osoba nenalezena");

			return await base.ValidateAsync(context, cancellationToken);
		}
	}
}
