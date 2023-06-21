using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using ODF.AppLayer.CQRS.Lineup.Commands;
using ODF.AppLayer.Repos;

namespace ODF.AppLayer.CQRS.Lineup.CommandValidators
{
	public class UpdateLineupItemCommandValidator : AbstractValidator<UpdateLineupItemCommand>
	{
		private readonly ILineupRepo _repo;
		private readonly IUserRepo _userRepo;

		public UpdateLineupItemCommandValidator(ILineupRepo repo, IUserRepo userRepo)
		{
			_repo = repo ?? throw new ArgumentNullException(nameof(repo));
			_userRepo = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
		}

		public override async Task<ValidationResult> ValidateAsync(ValidationContext<UpdateLineupItemCommand> context, CancellationToken cancellation = default)
		{
			var item = await _repo.GetAsync(context.InstanceToValidate.Id, cancellation);
			bool exists = item is not null;

			if (!string.IsNullOrEmpty(context.InstanceToValidate.DescriptionTranslationCode))
			{
				RuleFor(command => command.DescriptionTranslationCode)
				.Must(command => context.InstanceToValidate.DescriptionTranslationCode == item.DescriptionTranslation)
				.WithMessage("Nedělej v tom bordel a nech tu překladovku jak byla");
			}

			RuleFor(command => command.Id)
				.Must(command => exists)
				.WithMessage("Nenalezen záznam se zadaným Id");

			if (!string.IsNullOrEmpty(context.InstanceToValidate.UserName))
			{
				var user = await _userRepo.GetUserAsync(context.InstanceToValidate.UserName, cancellation);

				RuleFor(command => command.UserName)
					.Must(command => user is not null)
					.WithMessage("Událost nelze přiřadit neexistujícímu uživateli");

				RuleFor(command => command.UserName)
					.Must(command => user.IsActive)
					.WithMessage("Uživatel musí být aktivován, aby se dal používat na tyhle čupr finty");
			}

			return await base.ValidateAsync(context, cancellation);
		}
	}
}
