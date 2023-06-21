using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using ODF.AppLayer.CQRS.Lineup.Commands;
using ODF.AppLayer.Repos;

namespace ODF.AppLayer.CQRS.Lineup.CommandValidators
{
	internal class DeleteLineupItemCommandValidator : AbstractValidator<DeleteLineupItemCommand>
	{
		private readonly ILineupRepo _repo;

		public DeleteLineupItemCommandValidator(ILineupRepo repo)
		{
			_repo = repo ?? throw new ArgumentNullException(nameof(repo));
		}

		public override async Task<ValidationResult> ValidateAsync(ValidationContext<DeleteLineupItemCommand> context, CancellationToken cancellation = default)
		{
			bool exists = (await _repo.GetAsync(context.InstanceToValidate.Id, cancellation)) is not null;

			RuleFor(command => command.Id)
				.Must(command => exists)
				.WithMessage("Nenalezen záznam se zadaným Id");

			return await base.ValidateAsync(context, cancellation);
		}
	}
}
