using ODF.AppLayer.Dtos.ContactDtos;
using ODF.AppLayer.Mediator;

namespace ODF.AppLayer.CQRS.Contact.Queries
{
	public sealed record GetContactQuery(string CountryCode) : IQuery<ContactDto>;
}
