using MediatR;
using ODF.AppLayer.Dtos.ContactDtos;

namespace ODF.AppLayer.CQRS.Contact.Queries
{
	public class GetContactQuery : IRequest<ContactDto>
	{
		public GetContactQuery(string countryCode)
		{
			CountryCode = countryCode;
		}

		public string CountryCode { get; }
	}
}
