using ODF.API.RequestModels.Forms.Contacts;
using ODF.AppLayer.Dtos.ContactDtos;

namespace ODF.API.Extensions.MappingExtensions
{
	public static class ContactDtoToFormMapper
	{
		public static UpdateAddressForm ToForm(this AddressDto dto)
			=> new()
			{
				City = dto.City,
				PostalCode = dto.PostalCode,
				Country = dto.Country,
				Street = dto.Street,
			};

		public static UpdateContactForm ToForm(this ContactDto dto)
			=> new()
			{
				Email = dto.Email,
				EventManager = dto.EventManager,
				EventName = dto.EventName,
			};
	}
}
