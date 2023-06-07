using ODF.AppLayer.Dtos.ContactDtos;
using ODF.AppLayer.Mediator;

namespace ODF.AppLayer.CQRS.Contact.Queries
{
	public class GetContactQuery : IQuery<ContactDto>
	{
		public GetContactQuery(string countryCode)
		{
			CountryCode = countryCode;
		}

		public string CountryCode { get; }
	}
}
