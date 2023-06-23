using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using ODF.AppLayer.CQRS.Contact.Commands;
using ODF.Domain.Extensions;

namespace ODF.AppLayer.CQRS.Contact.CommandValidators
{
	public class UpdateContactCommandValidator : AbstractValidator<UpdateContactCommand>
	{
		public override async Task<ValidationResult> ValidateAsync(ValidationContext<UpdateContactCommand> context, CancellationToken cancellation = default)
		{
			RuleFor(contact => contact.Email)
				.Must(email => string.IsNullOrEmpty(email) || email.ValidateEmail())
				.WithMessage("Byl zadán nevalidní e-mail");

			return await base.ValidateAsync(context, cancellation);
		}
	}
}
