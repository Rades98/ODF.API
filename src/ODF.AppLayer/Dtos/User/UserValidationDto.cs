using System.Collections.Generic;
using FluentValidation.Results;
using ODF.AppLayer.Dtos.Interfaces;

namespace ODF.AppLayer.Dtos.User
{
	public class UserValidationDto : IValidationDto
	{
		public UserDto User { get; set; } = new();

		public bool IsOk { get; set; }

		public IEnumerable<ValidationFailure> Errors { get; set; }
	}
}
