using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ODF.Domain.Entities;

namespace ODF.AppLayer.Repos
{
	public interface IUserRepo
	{
		public Task<User> GetUserAsync(string userName, CancellationToken cancellationToken);

		public Task<User> GetUserByHashAsync(string hash, CancellationToken cancellationToken);

		public Task<string> RegisterAsync(string userName, string email, bool isAdmin, string passwordHash, CancellationToken cancellationToken);

		public Task<bool> ChangePasswordAsync(string oldPwHash, string oldPwSalt, string newPwHash, string newPwSalt, CancellationToken cancellationToken);

		public Task<IEnumerable<string>> GetUserNamesAsync(CancellationToken cancellationToken);

		public Task<IEnumerable<string>> GetUserEmailsAsync(CancellationToken cancellationToken);

		Task<bool> ActivateRegistration(string userName, CancellationToken cancellationToken);
	}
}
