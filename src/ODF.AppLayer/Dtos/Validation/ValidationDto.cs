using System.Collections.Generic;
using FluentValidation.Results;

namespace ODF.AppLayer.Dtos.Validation
{
	public class ValidationDto
	{
		public ValidationDto(bool isOk, IEnumerable<ValidationFailure> errors = null)
		{
			IsOk = isOk;
			Errors = errors;
		}

		public bool IsOk { get; }

		public IEnumerable<ValidationFailure> Errors { get; }
	}
}
