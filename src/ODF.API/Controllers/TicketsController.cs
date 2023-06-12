using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using ODF.API.Controllers.Base;
using ODF.API.Registration.SettingModels;
using ODF.AppLayer.Services.Interfaces;

namespace ODF.API.Controllers
{
	public class TicketsController : BaseController
	{
		public TicketsController(IMediator mediator, IOptions<ApiSettings> apiSettings, IActionDescriptorCollectionProvider adcp, ITranslationsProvider translationsProvider) : base(mediator, apiSettings, adcp, translationsProvider)
		{
		}

		[HttpGet(Name = nameof(GetTickets))]
		public IActionResult GetTickets([FromRoute] string countryCode, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
			//return Ok();
		}
	}
}
