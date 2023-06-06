using System.Threading;
using System.Threading.Tasks;
using ODF.Domain.Entities;

namespace ODF.AppLayer.Repos
{
	public interface IUserRepo
	{
		public Task<User> GetUserAsync(string userName, CancellationToken cancellationToken);

		//This method should create inactive user with mail token, then mail token should be sent + after e-mail link visit this user should be activated
		public Task<bool> RegisterAsync(string userName, string email, bool isAdmin, string passwordHash, string passwordSalt);

		public Task<bool> ChangePasswordAsync(string oldPwHash, string oldPwSalt, string newPwHash, string newPwSalt, CancellationToken cancellationToken);
	}
}
