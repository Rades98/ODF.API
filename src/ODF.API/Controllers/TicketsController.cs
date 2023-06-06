using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using ODF.API.Controllers.Base;
using ODF.API.Registration.SettingModels;

namespace ODF.API.Controllers
{
	public class TicketsController : BaseController
	{
		public TicketsController(IMediator mediator, IOptions<ApiSettings> apiSettings, IActionDescriptorCollectionProvider adcp) : base(mediator, apiSettings, adcp)
		{
		}

		[HttpGet(Name = nameof(GetTickets))]
		public IActionResult GetTickets([FromRoute] string countryCode, CancellationToken cancellationToken)
		{
			return Ok();
		}
	}
}
