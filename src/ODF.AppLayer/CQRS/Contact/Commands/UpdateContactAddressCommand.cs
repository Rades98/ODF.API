using ODF.AppLayer.CQRS.Interfaces.Contact;
using ODF.AppLayer.Dtos.Validation;
using ODF.AppLayer.Mediator;

namespace ODF.AppLayer.CQRS.Contact.Commands
{
	public sealed class UpdateContactAddressCommand : ICommand<ValidationDto>, IUpdateContactAddress
	{
		public UpdateContactAddressCommand(string street, string city, string postalCode, string country)
		{
			Street = street;
			City = city;
			PostalCode = postalCode;
			Country = country;
		}

		public string Street { get; }

		public string City { get; }

		public string PostalCode { get; }

		public string Country { get; }
	}
}
