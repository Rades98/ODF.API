using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using ODF.API.Attributes.HtttpMethodAttributes;
using ODF.API.Controllers.Base;
using ODF.API.Extensions.MappingExtensions;
using ODF.API.FormFactories;
using ODF.API.ResponseModels.Lineup.Redaction;
using ODF.API.ResponseModels.Redaction;
using ODF.AppLayer.CQRS.Lineup.Queries;
using ODF.AppLayer.Services.Interfaces;
using ODF.Domain.Constants;
using ODF.Domain.SettingModels;

namespace ODF.API.Controllers.Lineup
{
	public class LineupRedactionController : BaseController
	{
		public LineupRedactionController(IMediator mediator, IOptions<ApiSettings> apiSettings, IActionDescriptorCollectionProvider adcp, ITranslationsProvider translationsProvider)
			: base(mediator, apiSettings, adcp, translationsProvider)
		{
		}

		[HttpGet(Name = nameof(GetLineupRedaction))]
		[Authorize(Roles = UserRoles.Admin)]
		[CountryCodeFilter("cz")]
		[ProducesResponseType(typeof(RedactionResponseModel), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetLineupRedaction([FromRoute] string countryCode, CancellationToken cancellationToken)
		{
			var lineupItems = await Mediator.Send(new GetLineupQuery(countryCode), cancellationToken);
			var responseModel = new GetLineupRedactionResponseModel();

			responseModel.AddLineupItem = GetNamedAction(nameof(LineupController.AddItemToLineup), $"Přidat item do programu", "add_lineup_item",
					LineupItemFormFactory.GetAddLineupItemForm(new()));

			responseModel.LineupItems = lineupItems
				.OrderBy(ord => ord.DateTime)
				.Select(x => new GetLineupItemRedactionResponseModel()
				{
					Date = x.DateTime.ToString("dd.MM.yyyy"),
					Description = x.Description,
					Interpret = x.Interpret,
					PerformanceName = x.PerformanceName,
					Time = x.DateTime.ToString("HH:mm"),
					DeleteLineupItem = GetNamedAction(nameof(LineupController.DeleteLineupItem), $"Odebrat z programu", "remove_lineup_item",
						LineupItemFormFactory.GetDeleteLineupItemForm(new() { Id = x.Id })),
					UpdateLineupItem = GetNamedAction(nameof(LineupController.UpdateLineupItem), $"Upravit", "update_ineup_item",
						LineupItemFormFactory.GetUdpateLineupItemForm(x.ToUpdateForm())),
				});

			return Ok(responseModel);
		}
	}
}
