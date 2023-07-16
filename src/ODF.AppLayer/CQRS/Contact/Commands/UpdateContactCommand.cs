using ODF.AppLayer.CQRS.Interfaces.Contact;
using ODF.AppLayer.Dtos.Validation;
using ODF.AppLayer.Mediator;

namespace ODF.AppLayer.CQRS.Contact.Commands
{
	public sealed class UpdateContactCommand : ICommand<ValidationDto>, IUpdateContact
	{
		public UpdateContactCommand(IUpdateContact input)
		{
			EventManager = input.EventManager;
			Email = input.Email;
			EventName = input.EventName;
		}

		public string EventName { get; }

		public string EventManager { get; }

		public string Email { get; }
	}
}
