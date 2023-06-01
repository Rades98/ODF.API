using System;

namespace ODF.Data.Contracts.Entities
{
	[Serializable]
	public class User
	{
		public Guid Id { get; set; } = Guid.NewGuid();

		public string UserName { get; set; } = string.Empty;

		public string Email { get; set; } = string.Empty;

		public string PasswordHash { get; set; } = string.Empty;

		public string PasswordSalt { get; set; } = string.Empty;

		public bool IsAdmin { get; set; } = false;

		public string FirstName { get; set; } = string.Empty;

		public string LastName { get; set; } = string.Empty;

		public bool IsActive { get; set; } = false;
	}
}
