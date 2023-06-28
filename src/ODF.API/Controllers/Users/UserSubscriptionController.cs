using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using ODF.API.Controllers.Base;
using ODF.AppLayer.Extensions;
using ODF.AppLayer.Services.Interfaces;
using ODF.Domain.SettingModels;

namespace ODF.API.Controllers.Users
{
	[Authorize]
	public class UserSubscriptionController : BaseController
	{
		public UserSubscriptionController(IMediator mediator, IOptions<ApiSettings> apiSettings, IActionDescriptorCollectionProvider adcp, ITranslationsProvider translationsProvider)
			: base(mediator, apiSettings, adcp, translationsProvider)
		{
		}

		[HttpGet(Name = nameof(GetSubscription))]
		public async Task<IActionResult> GetSubscription([FromRoute] string countryCode, CancellationToken cancellationToken)
		{
			var translations = await TranslationsProvider.GetTranslationsAsync(countryCode, cancellationToken);
			return Ok(translations.Get("work_in_progress"));
		}

		// TODO subscription management (e-mail)

		// Categories: 

		// program info

		// personalized performer info

		// subscription of articles

		// subscription of new web features

		// notification of want to C performance - lineup must have that choice
	}
}
