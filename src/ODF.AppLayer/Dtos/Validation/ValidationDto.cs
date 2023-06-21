using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using ODF.AppLayer.Dtos.Interfaces;

namespace ODF.AppLayer.Dtos.Validation
{
	public class ValidationDto : IValidationDto
	{
		public bool IsOk { get; set; }

		public IEnumerable<ValidationFailure> Errors { get; set; } = Enumerable.Empty<ValidationFailure>();

		public static ValidationDto Invalid
			=> new() { IsOk = false };
	}
}
