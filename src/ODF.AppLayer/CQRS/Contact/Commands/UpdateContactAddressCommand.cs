﻿using MediatR;

namespace ODF.AppLayer.CQRS.Contact.Commands
{
	public class UpdateContactAddressCommand : IRequest<bool>
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