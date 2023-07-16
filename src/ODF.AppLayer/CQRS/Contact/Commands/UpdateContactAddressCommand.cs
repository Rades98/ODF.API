using ODF.AppLayer.CQRS.Interfaces.Contact;
using ODF.AppLayer.Dtos.Validation;
using ODF.AppLayer.Mediator;

namespace ODF.AppLayer.CQRS.Contact.Commands
{
	public sealed class UpdateContactAddressCommand : ICommand<ValidationDto>, IUpdateContactAddress
	{
		public UpdateContactAddressCommand(IUpdateContactAddress input)
		{
			Street = input.Street;
			City = input.City;
			PostalCode = input.PostalCode;
			Country = input.Country;
		}

		public string Street { get; }

		public string City { get; }

		public string PostalCode { get; }

		public string Country { get; }
	}
}
