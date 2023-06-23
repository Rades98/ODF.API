using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using ODF.AppLayer.CQRS.Contact.Commands;

namespace ODF.AppLayer.CQRS.Contact.CommandValidators
{
	public class AddContactPersonCommandValidator : AbstractValidator<AddContactPersonCommand>
	{
		public override async Task<ValidationResult> ValidateAsync(ValidationContext<AddContactPersonCommand> context, CancellationToken cancellation = default)
		{

			RuleFor(command => command.Name)
				.Must(name => !string.IsNullOrEmpty(name))
				.WithMessage("Člověk nemá jméno? RLY?");

			RuleFor(command => command.Surname)
				.Must(surname => !string.IsNullOrEmpty(surname))
				.WithMessage("Člověk nemá příjmení?");

			RuleFor(command => command.Roles)
				.Must(roles => roles is not null && roles.Any())
				.WithMessage("Člověk je tu zbytečně? Bez jediné role?");

			RuleFor(command => command.Email)
				.Must(email => !string.IsNullOrEmpty(email))
				.WithMessage("Email jako co?");

			RuleFor(command => command.Base64Image)
				.Must(image => !string.IsNullOrEmpty(image))
				.WithMessage("Dej chudákovi obrázek :)");

			return await base.ValidateAsync(context, cancellation);
		}
	}
}
