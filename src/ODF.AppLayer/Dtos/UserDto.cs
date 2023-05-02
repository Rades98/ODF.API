using System.Collections.Generic;
using System.Security.Claims;

namespace ODF.AppLayer.Dtos
{
	public class UserDto
	{
		public string UserName { get; set; }

		public string Email { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public IEnumerable<Claim> Claims { get; set; }
	}
}
