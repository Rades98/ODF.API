using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using ODF.AppLayer.CQRS.Contact.Commands;

namespace ODF.AppLayer.CQRS.Contact.CommandValidators
{
	public class UpdateContactAddressCommandValidator : AbstractValidator<UpdateContactAddressCommand>
	{
		private readonly static Regex PostalCodeRegex = new(@"^(?:\d{3}\s\d{2})|(?:\d{5})$", RegexOptions.Compiled);

		public override async Task<ValidationResult> ValidateAsync(ValidationContext<UpdateContactAddressCommand> context, CancellationToken cancellation = default)
		{
			RuleFor(command => command.PostalCode)
				.NotNull()
				.NotEmpty()
				.WithMessage("PSČ musí být vyplněno");

			RuleFor(command => command.PostalCode)
				.Must(command => PostalCodeRegex.IsMatch(context.InstanceToValidate.PostalCode))
				.WithMessage("PSČ musí být ve formátu XXX XX nebo XXXXX");

			RuleFor(command => command.City)
				.NotNull()
				.NotEmpty()
				.WithMessage("Obec musí být vyplněna");

			RuleFor(command => command.Street)
				.NotNull()
				.NotEmpty()
				.WithMessage("Ulice musí být vyplněna");

			RuleFor(command => command.Country)
				.NotNull()
				.NotEmpty()
				.WithMessage("Země musí být vyplněna");

			return await base.ValidateAsync(context, cancellation);
		}
	}
}
