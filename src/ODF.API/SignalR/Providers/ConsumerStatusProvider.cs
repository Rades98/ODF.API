using System.Collections.Concurrent;
using ODF.API.SignalR.Interfaces;

namespace ODF.API.SignalR.Providers
{
	public class ConsumerStatusProvider : IConsumerStatusProvider
	{
		private readonly ConcurrentDictionary<string, bool> _userStatus = new();

		public void ChangeConsumerState(string consumer, bool isConnected)
		{
			if (_userStatus.TryGetValue(consumer, out _))
			{
				_userStatus[consumer] = isConnected;
			}
			else
			{
				_ = _userStatus.Append(new(consumer, isConnected));
			}
		}

		public bool IsConsumerConnected(string consumer)
		{
			if (_userStatus.TryGetValue(consumer, out _))
			{
				return _userStatus[consumer];
			}

			return false;
		}
	}
}
