using MediatR;
using ODF.AppLayer.Dtos.ContactDtos;

namespace ODF.AppLayer.CQRS.Contact.Queries
{
	public class GetContactQeury : IRequest<ContactDto>
	{
		public GetContactQeury(string countryCode)
		{
			CountryCode = countryCode;
		}

		public string CountryCode { get; }
	}
}
