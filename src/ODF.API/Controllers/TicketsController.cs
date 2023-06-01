﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ODF.API.Controllers.Base;
using ODF.API.Registration.SettingModels;

namespace ODF.API.Controllers
{
	public class TicketsController : BaseController
	{
		public TicketsController(IMediator mediator, IOptions<ApiSettings> apiSettings) : base(mediator, apiSettings)
		{
		}

		[HttpGet("/{countryCode}/tickets")]
		public IActionResult GetTickets([FromRoute] string countryCode, CancellationToken ct)
		{
			return Ok();
		}
	}
}