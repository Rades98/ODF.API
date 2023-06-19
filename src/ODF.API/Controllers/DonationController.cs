using System.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using ODF.API.Controllers.Base;
using ODF.API.ResponseModels.Donations;
using ODF.AppLayer.CQRS.Contact.Queries;
using ODF.AppLayer.Extensions;
using ODF.AppLayer.Services.Interfaces;
using ODF.Domain.SettingModels;

namespace ODF.API.Controllers
{
	public class DonationController : BaseController
	{
		public DonationController(IMediator mediator, IOptions<ApiSettings> apiSettings, IActionDescriptorCollectionProvider adcp, ITranslationsProvider translationsProvider) : base(mediator, apiSettings, adcp, translationsProvider)
		{
		}

		[HttpGet(Name = nameof(GetDonation))]
		public async Task<IActionResult> GetDonation([FromRoute] string countryCode, CancellationToken cancellationToken)
		{
			var translations = await TranslationsProvider.GetTranslationsAsync(countryCode, cancellationToken);
			var result = await Mediator.Send(new GetBankAccountsQRQuery(), cancellationToken);

			var responseModel = new DonationResponseModel(result.Select(x => x.ToString()), translations.Get("donations_header"), translations.Get("donations_text"));
			return Ok(responseModel);
		}
	}
}
