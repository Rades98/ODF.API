using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using ODF.API.SignalR.Interfaces;

namespace ODF.API.SignalR.Hubs
{
	public sealed class ChatHub : Hub
	{
		private readonly IConsumerStatusProvider _consumerStatusProvider;
		private readonly ILogger _logger;

		public ChatHub(IConsumerStatusProvider consumerStatusProvider, ILogger<ChatHub> logger)
		{
			_consumerStatusProvider = consumerStatusProvider ?? throw new ArgumentNullException(nameof(consumerStatusProvider));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public override async Task OnConnectedAsync()
		{
			string? name = Context.User?.FindFirstValue(ClaimTypes.Name);

			if (name is not null)
			{
				_logger.LogInformation("[SignalR] connected user with name: {name}", name);
				_consumerStatusProvider.ChangeConsumerState(name, true);
			}

			await base.OnConnectedAsync();
		}

		public override Task OnDisconnectedAsync(Exception? exception)
		{
			string? name = Context.User?.FindFirstValue(ClaimTypes.Name);
			_logger.LogInformation("[SignalR] user with name: {name} disconnected", name);

			if (name is not null)
			{
				_consumerStatusProvider.ChangeConsumerState(name, false);
			}

			return base.OnDisconnectedAsync(exception);
		}
	}
}
