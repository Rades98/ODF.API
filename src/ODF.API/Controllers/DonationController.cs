using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using ODF.API.Controllers.Base;
using ODF.API.ResponseModels.Donations;
using ODF.API.ResponseModels.Exceptions;
using ODF.AppLayer.CQRS.Contact.Queries;
using ODF.AppLayer.Extensions;
using ODF.AppLayer.Services.Interfaces;
using ODF.Domain.SettingModels;

namespace ODF.API.Controllers
{
	public class DonationController : BaseController
	{
		public DonationController(IMediator mediator, IOptions<ApiSettings> apiSettings, IActionDescriptorCollectionProvider adcp, ITranslationsProvider translationsProvider)
			: base(mediator, apiSettings, adcp, translationsProvider)
		{
		}

		[HttpGet(Name = nameof(GetDonation))]
		[ProducesResponseType(typeof(DonationResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetDonation(CancellationToken cancellationToken)
		{
			var translations = await TranslationsProvider.GetTranslationsAsync(CountryCode, cancellationToken);
			var result = await Mediator.Send(new GetBankAccountsQRQuery(), cancellationToken);

			var qrData = Enumerable.Empty<DonationBankAccResponseModel>();

			if (result.Any())
			{
				qrData = result.Select(x => new DonationBankAccResponseModel()
				{
					AccountId = x.AccountId,
					AccountIdTranslation = translations.Get("contact_bank"),
					IBAN = x.IBAN,
					IBANTranslation = translations.Get("contact_iban"),
					QrString = x.ToString()
				});
			}

			var responseModel = new DonationResponseModel(qrData, translations.Get("donations_header"), translations.Get("donations_text"), translations.Get("donations_text2"));
			return Ok(responseModel);
		}
	}
}
