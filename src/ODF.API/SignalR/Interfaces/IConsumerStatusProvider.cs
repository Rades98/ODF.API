namespace ODF.API.SignalR.Interfaces
{
	public interface IConsumerStatusProvider
	{
		void ChangeConsumerState(string consumer, bool isConnected);
		bool IsConsumerConnected(string consumer);
	}
}
