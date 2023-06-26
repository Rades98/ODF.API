using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using ODF.AppLayer.CQRS.Article.Commands;

namespace ODF.AppLayer.CQRS.Article.CommandValidators
{
	public class AddArticleCommandValidator : AbstractValidator<AddArticleCommand>
	{
		public override async Task<ValidationResult> ValidateAsync(ValidationContext<AddArticleCommand> context, CancellationToken cancellation = default)
		{
			RuleFor(article => article.TitleTranslationCode)
				.NotEmpty()
				.NotNull()
				.WithMessage("Není vyplněn název překladové proměnné pro nadpis");

			RuleFor(article => article.TextTranslationCode)
				.NotEmpty()
				.NotNull()
				.WithMessage("Není vyplněn název překladové proměnné pro text");

			RuleFor(article => article.Title)
				.NotEmpty()
				.NotNull()
				.WithMessage("Jooooooo, článek bez nadpisu");

			RuleFor(article => article.Text)
				.NotEmpty()
				.NotNull()
				.WithMessage("a TeXT jE KdE?");

			return await base.ValidateAsync(context, cancellation);
		}
	}
}
