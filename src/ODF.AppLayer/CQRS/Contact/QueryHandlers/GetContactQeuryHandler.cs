using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ODF.AppLayer.CQRS.Contact.Queries;
using ODF.AppLayer.Dtos.ContactDtos;
using ODF.AppLayer.Extensions;
using ODF.AppLayer.Mapping;
using ODF.AppLayer.Mediator;
using ODF.AppLayer.Repos;
using ODF.AppLayer.Services.Interfaces;

namespace ODF.AppLayer.CQRS.Contact.QueryHandlers
{
	internal class GetContactQeuryHandler : IQueryHandler<GetContactQuery, ContactDto>
	{
		private readonly ITranslationsProvider _translationProvider;
		private readonly IContactRepo _contactRepo;

		public GetContactQeuryHandler(IContactRepo contactRepo, ITranslationsProvider translationsProvider)
		{
			_contactRepo = contactRepo ?? throw new ArgumentNullException(nameof(contactRepo));
			_translationProvider = translationsProvider ?? throw new ArgumentNullException(nameof(translationsProvider));
		}

		public async Task<ContactDto> Handle(GetContactQuery request, CancellationToken cancellationToken)
		{
			var translations = await _translationProvider.GetTranslationsAsync(request.CountryCode, cancellationToken);
			var contact = await _contactRepo.GetAsync(cancellationToken);

			if (contact is null)
			{
				return null;
			}

			return new()
			{
				Address = contact.Address.MapAddress(),
				BankAccounts = contact.BankAccounts.Select(ba => ba.MapBankAccount(translations.Get("contact_acc_id"), translations.Get("contact_bank"), translations.Get("contact_iban"))),
				ContactPersons = contact.ContactPersons.OrderBy(ord => ord.Order).Select(cp => cp.MapContactPerson()),
				Email = contact.Email,
				EmailTranslation = translations.Get("contact_email"),
				EventManager = contact.EventManager,
				EventManagerTranslation = translations.Get("contact_event_manager"),
				EventName = contact.EventName
			};
		}
	}
}
