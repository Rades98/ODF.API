using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using ODF.AppLayer.CQRS.Contact.Commands;
using ODF.AppLayer.Repos;
using ODF.Domain.Extensions;

namespace ODF.AppLayer.CQRS.Contact.CommandValidators
{
	public class UpdateContactCommandValidator : AbstractValidator<UpdateContactCommand>
	{
		private readonly IContactRepo _contactRepo;

		public UpdateContactCommandValidator(IContactRepo contactRepo)
		{
			_contactRepo = contactRepo ?? throw new ArgumentNullException(nameof(contactRepo));
		}

		public override async Task<ValidationResult> ValidateAsync(ValidationContext<UpdateContactCommand> context, CancellationToken cancellation = default)
		{
			var contact = await _contactRepo.GetAsync(cancellation);

			RuleFor(contact => contact.Email)
				.Must(cont => context.InstanceToValidate.Email.ValidateEmail())
				.WithMessage("Byl zadán nevalidní e=mail");

			return await base.ValidateAsync(context, cancellation);
		}
	}
}
