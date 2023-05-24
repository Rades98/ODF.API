﻿using MediatR;

namespace ODF.AppLayer.CQRS.Contact.Commands
{
	public class UpdateContactCommand : IRequest<bool>
	{
		public UpdateContactCommand(string eventName, string eventManager, string email)
		{
			EventManager = eventManager;
			Email = email;
			EventName = eventName;
		}

		public string EventName { get; }

		public string EventManager { get; }

		public string Email { get; }
	}
}
