using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using ODF.API.Attributes.HtttpMethodAttributes;
using ODF.API.Controllers.Base;
using ODF.API.Extensions.MappingExtensions;
using ODF.API.FormComposers;
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
		public async Task<IActionResult> GetLineupRedaction(CancellationToken cancellationToken)
		{
			var lineupItems = await Mediator.Send(new GetLineupQuery(CountryCode), cancellationToken);
			var responseModel = new GetLineupRedactionResponseModel();

			responseModel.AddLineupItem = GetNamedAction(nameof(LineupController.AddItemToLineup), $"Přidat item do programu", "add_lineup_item",
					LineupItemFormComposer.GetAddLineupItemForm(new() { DateTime = DateTime.Now }, usersDataSource: GetAppAction(nameof(DataSourceController.GetUsers), "get_users_source")));
			responseModel.RedactionPartName = "Úprava programu";

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
						LineupItemFormComposer.GetDeleteLineupItemForm(new() { Id = x.Id })),
					UpdateLineupItem = GetNamedAction(nameof(LineupController.UpdateLineupItem), $"Upravit", "update_ineup_item",
						LineupItemFormComposer.GetUdpateLineupItemForm(x.ToUpdateForm(), usersDataSource: GetAppAction(nameof(DataSourceController.GetUsers), "get_users_source"))),
					UserName = x.UserName,
					UserNote = x.UserNote,
				});

			return Ok(responseModel);
		}
	}
}
