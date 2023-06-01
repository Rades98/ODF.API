using System.Collections.Generic;
using FluentValidation.Results;
using ODF.AppLayer.Dtos.Validation;

namespace ODF.AppLayer.Dtos.User
{
	public class UserValidationDto : ValidationDto
	{
		public UserValidationDto(bool isOk, IEnumerable<ValidationFailure> errors = null) : base(isOk, errors)
		{
		}

		public UserDto User { get; set; }
	}
}
