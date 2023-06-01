using System.Collections.Generic;
using System.Security.Claims;

namespace ODF.AppLayer.Dtos.User
{
	public class UserDto
	{
		public string UserName { get; set; } = string.Empty;

		public string Email { get; set; } = string.Empty;

		public string FirstName { get; set; } = string.Empty;

		public string LastName { get; set; } = string.Empty;

		public IEnumerable<Claim> Claims { get; set; }
	}
}
