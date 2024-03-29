﻿using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using ODF.AppLayer.CQRS.Lineup.Commands;
using ODF.AppLayer.Repos;
using ODF.Domain;

namespace ODF.AppLayer.CQRS.Lineup.CommandValidators
{
	public class AddLineupItemCommandValidator : AbstractValidator<AddLineupItemCommand>
	{
		private readonly IUserRepo _userRepo;
		private readonly ITranslationRepo _translationRepo;

		public AddLineupItemCommandValidator(IUserRepo userRepo, ITranslationRepo translationRepo)
		{
			_userRepo = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
			_translationRepo = translationRepo ?? throw new ArgumentNullException(nameof(translationRepo));
		}

		public override async Task<ValidationResult> ValidateAsync(ValidationContext<AddLineupItemCommand> context, CancellationToken cancellation = default)
		{
			bool transCodeExists = !string.IsNullOrEmpty((await _translationRepo.GetTranslationAsync(context.InstanceToValidate.DescriptionTranslationCode, Languages.Czech.Id, cancellation)));

			if (!string.IsNullOrEmpty(context.InstanceToValidate.UserName))
			{
				var user = await _userRepo.GetUserAsync(context.InstanceToValidate.UserName, cancellation);

				RuleFor(command => command.UserName)
					.Must(command => user is not null)
					.WithMessage("Událost nelze přiřadit neexistujícímu uživateli");

				RuleFor(command => command.UserName)
					.Must(command => user?.IsActive ?? false)
					.WithMessage("Uživatel musí být aktivován, aby se dal používat na tyhle čupr finty");
			}

			RuleFor(command => command.DateTime)
				.NotNull()
				.WithMessage("Datum a čas musí být vyplněny");

			RuleFor(command => command.PerformanceName)
				.NotNull()
				.NotEmpty()
				.WithMessage("Název vystoupení musí být vyplněn");

			RuleFor(command => command.Place)
				.NotNull()
				.NotEmpty()
				.WithMessage("Místo musí být vyplněno");

			RuleFor(command => command.Description)
				.NotNull()
				.NotEmpty()
				.WithMessage("Popis musí být vyplněn");

			RuleFor(command => command.DescriptionTranslationCode)
				.NotNull()
				.NotEmpty()
				.WithMessage("Překladová proměnná pro popis musí být vyplněna");

			RuleFor(command => command.DescriptionTranslationCode)
				.Must(code => !transCodeExists)
				.WithMessage("Název překladové proměnné už je obsazen");

			RuleFor(command => command.Interpret)
				.NotNull()
				.NotEmpty()
				.WithMessage("Interpret musí být vyplněn");

			return await base.ValidateAsync(context, cancellation);
		}
	}
}
