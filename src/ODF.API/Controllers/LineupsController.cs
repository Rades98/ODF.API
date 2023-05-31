﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ODF.API.Registration.SettingModels;
using ODF.API.RequestModels.Forms;
using ODF.API.ResponseModels.About;
using ODF.API.ResponseModels.Exceptions;
using ODF.API.ResponseModels.Lineup;
using ODF.API.Responses;
using ODF.AppLayer.Consts;
using ODF.AppLayer.CQRS.Lineup.Commands;
using ODF.AppLayer.CQRS.Lineup.Queries;
using ODF.AppLayer.CQRS.Translations.Queries;
using System.Data;

namespace ODF.API.Controllers
{
    public class LineupsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ApiSettings _settings;

        public LineupsController(IMediator mediator, IOptions<ApiSettings> apiSettings)
        {
            _mediator = mediator;
            _settings = apiSettings.Value;
        }


        [HttpGet("/{countryCode}/lineups")]
        [ProducesResponseType(typeof(LineupResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLineups([FromRoute] string countryCode, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetLineupQuery(countryCode), cancellationToken);

            var responseModel = new LineupResponseModel(_settings.ApiUrl, countryCode);

            responseModel.Lineup = result.OrderBy(ord => ord.DateTime)
                .GroupBy(o => o.Place)
                .ToDictionary(val => val.Key, val => val.Select(x => new LineupItemResponseModel()
                {
                    Date = x.DateTime.ToString("dd.MM.yyyy"),
                    Description = x.Description,
                    Interpret = x.Interpret,
                    PerformanceName = x.PerformanceName,
                    Time = x.DateTime.ToString("HH:mm")
                }));

            return Ok(responseModel);
        }

        [HttpPut("/{countryCode}/lineup")]
        [Authorize(Roles = UserRoles.Admin)]
        [ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<IActionResult> AddLineup([FromRoute] string countryCode, [FromBody] AddLineupItemForm model, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new AddLineupItemCommand(
                model.Place, model.Interpret, model.PerformanceName,
                model.Description, model.DescriptionTranslationCode, model.DateTime, countryCode), cancellationToken);

            if (result)
            {
                return Accepted();
            }

            return (IActionResult)CustomApiResponses.InternalServerError(new ExceptionResponseModel("Vyskytla se chyba při tvorbě události"));
        }
    }
}
