using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace ODF.API.SignalR.Providers
{
	public class CustomUserIdProvider : IUserIdProvider
	{
		public string? GetUserId(HubConnectionContext connection)
		{
			return connection.User?.FindFirst(ClaimTypes.Name)?.Value;
		}
	}
}
