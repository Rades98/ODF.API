using System.Collections.Generic;
using FluentValidation.Results;

namespace ODF.AppLayer.Dtos.Interfaces
{
	public interface IValidationDto
	{
		public bool IsOk { get; set; }

		public IEnumerable<ValidationFailure> Errors { get; set; }
	}
}
