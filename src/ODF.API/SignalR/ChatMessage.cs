namespace ODF.API.SignalR
{
	//This shit is for test purpose only...
	public class ChatMessage
	{
		public ChatMessage(string from, string to, string body)
		{
			From = from;
			To = to;
			Body = body;
		}

		public string From { get; }
		public string To { get; }
		public string Body { get; }
	}
}
