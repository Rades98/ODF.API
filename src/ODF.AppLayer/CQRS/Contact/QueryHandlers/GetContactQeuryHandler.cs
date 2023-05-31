using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ODF.AppLayer.CQRS.Contact.Queries;
using ODF.AppLayer.Dtos.ContactDtos;
using ODF.AppLayer.Mapping;
using ODF.Data.Contracts.Interfaces;
using ODF.Enums;

namespace ODF.AppLayer.CQRS.Contact.QueryHandlers
{
	internal class GetContactQeuryHandler : IRequestHandler<GetContactQuery, ContactDto>
	{
		private readonly ITranslationRepo _translationRepo;
		private readonly IContactRepo _contactRepo;

		public GetContactQeuryHandler(IContactRepo contactRepo, ITranslationRepo translationRepo)
		{
			_contactRepo = contactRepo ?? throw new ArgumentNullException(nameof(contactRepo));
			_translationRepo = translationRepo ?? throw new ArgumentNullException(nameof(translationRepo));
		}

		public async Task<ContactDto> Handle(GetContactQuery request, CancellationToken cancellationToken)
		{
			if (Languages.TryParse(request.CountryCode, out var lang))
			{
				var contact = await _contactRepo.GetAsync(cancellationToken);

				if (contact is null)
				{
					return null;
				}

				var accountIdTranslation = await _translationRepo.GetTranslationOrDefaultTextAsync("acc_id", "Festival Ostravské dny folkloru", lang.Id, cancellationToken);
				var bankTranslation = await _translationRepo.GetTranslationOrDefaultTextAsync("bank", "Bankovní spojení", lang.Id, cancellationToken);
				var ibanTranslation = await _translationRepo.GetTranslationOrDefaultTextAsync("iban", "IBAN", lang.Id, cancellationToken);
				var emailTranslation = await _translationRepo.GetTranslationOrDefaultTextAsync("email_contact", "Kontakt", lang.Id, cancellationToken);
				var eventManagerTranslation = await _translationRepo.GetTranslationOrDefaultTextAsync("event_manager_contact", "Pořadatel", lang.Id, cancellationToken);

				return new()
				{
					Address = contact.Address.MapAddress(),
					BankAccounts = contact.BankAccounts.Select(ba => ba.MapBankAccount(accountIdTranslation, bankTranslation, ibanTranslation)),
					ContactPersons = contact.ContactPersons.OrderBy(ord => ord.Order).Select(cp => cp.MapContactPerson()),
					Email = contact.Email,
					EmailTranslation = emailTranslation,
					EventManager = contact.EventManager,
					EventManagerTranslation = eventManagerTranslation,
					EventName = contact.EventName
				};
			}

			return null;
		}
	}
}
