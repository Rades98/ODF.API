using FluentValidation.Results;

namespace ODF.API.Extensions
{
	public static class ValidationFailureExtensions
	{
		public static string? GetErrorMessage(this IEnumerable<ValidationFailure> errors, string propName)
			=> errors?.FirstOrDefault(p => p.PropertyName.ToLower() == propName.ToLower())?.ErrorMessage;
	}
}
