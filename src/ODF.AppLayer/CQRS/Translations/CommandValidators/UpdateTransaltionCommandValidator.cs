using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using ODF.AppLayer.CQRS.Translations.Commands;
using ODF.AppLayer.Repos;

namespace ODF.AppLayer.CQRS.Translations.CommandValidators
{
	public class UpdateTransaltionCommandValidator : AbstractValidator<UpdateTransaltionCommand>
	{
		private readonly ITranslationRepo _translationRepo;

		public UpdateTransaltionCommandValidator(ITranslationRepo translationRepo)
		{
			_translationRepo = translationRepo ?? throw new ArgumentNullException(nameof(translationRepo));
		}

		public override async Task<ValidationResult> ValidateAsync(ValidationContext<UpdateTransaltionCommand> context, CancellationToken cancellation = default)
		{
			string existing = await _translationRepo.GetTranslationAsync(context.InstanceToValidate.TranslationCode, 0, cancellation);
			RuleFor(trans => trans.TranslationCode)
				.Must(code => !string.IsNullOrEmpty(existing))
				.WithMessage("Překladová proměnná nebzla nalezena");

			return await base.ValidateAsync(context, cancellation);
		}
	}
}
