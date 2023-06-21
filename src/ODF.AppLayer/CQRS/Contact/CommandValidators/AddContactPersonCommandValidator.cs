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
				.NotEmpty()
				.NotNull()
				.WithMessage("Člověk nemá jméno? RLY?");

			RuleFor(command => command.Surname)
				.NotEmpty()
				.NotNull()
				.WithMessage("Člověk nemá příjmení?");

			RuleFor(command => command.Roles)
				.NotEmpty()
				.NotNull()
				.WithMessage("Člověk je tu zbytečně? Bez jediné role?");

			RuleFor(command => command.Email)
				.NotEmpty()
				.NotNull()
				.WithMessage("Email jako co?");

			RuleFor(command => command.Base64Image)
				.NotEmpty()
				.NotNull()
				.WithMessage("Dej chudákovi obrázek :)");

			return await base.ValidateAsync(context, cancellation);
		}
	}
}
