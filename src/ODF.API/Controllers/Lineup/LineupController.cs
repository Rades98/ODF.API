using System.Data;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using ODF.API.Attributes.HtttpMethodAttributes;
using ODF.API.Controllers.Base;
using ODF.API.FormFactories;
using ODF.API.RequestModels.Forms.Lineup;
using ODF.API.ResponseModels.Exceptions;
using ODF.API.ResponseModels.Lineup;
using ODF.AppLayer.CQRS.Lineup.Commands;
using ODF.AppLayer.CQRS.Lineup.Queries;
using ODF.AppLayer.Services.Interfaces;
using ODF.Domain.Constants;
using ODF.Domain.SettingModels;

namespace ODF.API.Controllers.Lineup
{
	public class LineupController : BaseController
	{
		public LineupController(IMediator mediator, IOptions<ApiSettings> apiSettings, IActionDescriptorCollectionProvider adcp, ITranslationsProvider translationsProvider)
			: base(mediator, apiSettings, adcp, translationsProvider)
		{
		}

		[HttpGet(Name = nameof(GetLineup))]
		[ProducesResponseType(typeof(LineupResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetLineup([FromRoute] string countryCode, CancellationToken cancellationToken)
		{
			var result = await Mediator.Send(new GetLineupQuery(countryCode), cancellationToken);

			var responseModel = new LineupResponseModel();

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

		[HttpPut(Name = nameof(AddItemToLineup))]
		[Authorize(Roles = UserRoles.Admin)]
		[CountryCodeFilter("cz")]
		[ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(typeof(AddLineupResponseModel), StatusCodes.Status422UnprocessableEntity)]
		[ProducesResponseType(StatusCodes.Status202Accepted)]
		public async Task<IActionResult> AddItemToLineup([FromRoute] string countryCode, [FromBody] AddLineupItemForm form, CancellationToken cancellationToken)
		{
			var validationResult = await Mediator.Send(new AddLineupItemCommand(
				form.Place, form.Interpret, form.PerformanceName,
				form.Description, form.DescriptionTranslationCode, form.DateTime, countryCode), cancellationToken);

			if (validationResult.IsOk)
			{
				return Ok(new AddLineupResponseModel("Přidání položky do programu proběhlo úspěšně"));
			}

			if (validationResult.Errors.Any())
			{
				return UnprocessableEntity(new AddLineupResponseModel(LineupItemFormComposer.GetAddLineupItemForm(form, validationResult.Errors)));
			}

			return InternalServerError(new ExceptionResponseModel("Vyskytla se chyba při tvorbě události"));
		}

		[HttpPost(Name = nameof(UpdateLineupItem))]
		[Authorize(Roles = UserRoles.Admin)]
		[CountryCodeFilter("cz")]
		[ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(typeof(AddLineupResponseModel), StatusCodes.Status422UnprocessableEntity)]
		[ProducesResponseType(StatusCodes.Status202Accepted)]
		public async Task<IActionResult> UpdateLineupItem([FromRoute] string countryCode, [FromBody] UpdateLineupItemForm form, CancellationToken cancellationToken)
		{
			var validationResult = await Mediator.Send(
				new UpdateLineupItemCommand(form.Id, form.Place, form.Interpret, form.PerformanceName, form.Description,
				form.DescriptionTranslationCode, form.DateTime, countryCode, form.UserName), cancellationToken);

			if (validationResult.IsOk)
			{
				return Ok(new UpdateLineupResponseModel("Úprava položky z programu proběhlo úspěšně"));
			}

			if (validationResult.Errors.Any())
			{
				return UnprocessableEntity(new UpdateLineupResponseModel(LineupItemFormComposer.GetUdpateLineupItemForm(form, validationResult.Errors)));
			}

			return InternalServerError(new ExceptionResponseModel("Vyskytla se chyba při tvorbě události"));
		}

		[HttpDelete(Name = nameof(DeleteLineupItem))]
		[Authorize(Roles = UserRoles.Admin)]
		[CountryCodeFilter("cz")]
		[ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(typeof(AddLineupResponseModel), StatusCodes.Status422UnprocessableEntity)]
		[ProducesResponseType(StatusCodes.Status202Accepted)]
		public async Task<IActionResult> DeleteLineupItem([FromBody] DeleteLineupItemForm form, CancellationToken cancellationToken)
		{
			var validationResult = await Mediator.Send(new DeleteLineupItemCommand(form.Id), cancellationToken);

			if (validationResult.IsOk)
			{
				return Ok(new DeleteLineupResponseModel("Odebrání položky z programu proběhlo úspěšně"));
			}

			if (validationResult.Errors.Any())
			{
				return UnprocessableEntity(new DeleteLineupResponseModel(LineupItemFormComposer.GetDeleteLineupItemForm(form, validationResult.Errors)));
			}

			return InternalServerError(new ExceptionResponseModel("Vyskytla se chyba při tvorbě události"));
		}
	}
}
