using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ODF.API.Registration.SettingModels;
using ODF.API.ResponseModels.About;
using ODF.API.ResponseModels.Exceptions;
using ODF.AppLayer.CQRS.Translations.Queries;

namespace ODF.API.Controllers
{
    public class TicketsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ApiSettings _settings;

        public TicketsController(IMediator mediator, IOptions<ApiSettings> apiSettings)
        {
            _mediator = mediator;
            _settings = apiSettings.Value;
        }


        [HttpGet("/{countryCode}/tickets")]
        public IActionResult GetTickets([FromRoute] string countryCode, CancellationToken ct)
        {
            return Ok(); 
        }
    }
}
