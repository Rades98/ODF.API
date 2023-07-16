using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using ODF.API.Controllers.Base;
using ODF.API.Extensions;
using ODF.API.ResponseModels.Exceptions;
using ODF.API.ResponseModels.User;
using ODF.AppLayer.CQRS.Lineup.Queries;
using ODF.AppLayer.Extensions;
using ODF.AppLayer.Services.Interfaces;
using ODF.Domain.SettingModels;

namespace ODF.API.Controllers.Users
{
	[Authorize]
	public class UserMenuController : BaseController
	{
		public UserMenuController(IMediator mediator, IOptions<ApiSettings> apiSettings, IActionDescriptorCollectionProvider adcp, ITranslationsProvider translationsProvider)
			: base(mediator, apiSettings, adcp, translationsProvider)
		{
		}

		[HttpGet(Name = nameof(GetMenu))]
		[ProducesResponseType(typeof(UserMenuResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetMenu(CancellationToken cancellationToken)
		{
			var translations = await TranslationsProvider.GetTranslationsAsync(CountryCode, cancellationToken);
			var responseModel = new UserMenuResponseModel();
			responseModel.UserLineupAction = GetNamedAction(nameof(GetUserLineup), translations.Get("user_menu_lineup"), "user_lineup");
			responseModel.UserSubscriptionAction = GetNamedAction(nameof(UserSubscriptionController.GetSubscription), translations.Get("user_menu_subscription"), "user_subscription");

			return Ok(responseModel);
		}

		[HttpGet("lineup", Name = nameof(GetUserLineup))]
		[ProducesResponseType(typeof(UserLineupResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetUserLineup(CancellationToken cancellationToken)
		{
			var result = await Mediator.Send(new GetUserLineupQuery(HttpContext.GetUserName(), CountryCode), cancellationToken);

			var responseModel = new UserLineupResponseModel();

			responseModel.Lineup = result.OrderBy(ord => ord.DateTime)
				.GroupBy(o => o.Place)
				.ToDictionary(val => val.Key, val => val.Select(x => new UserLineupItemResponseModel()
				{
					Date = x.DateTime.ToString("dd.MM.yyyy"),
					Description = x.Description,
					Interpret = x.Interpret,
					PerformanceName = x.PerformanceName,
					Time = x.DateTime.ToString("HH:mm"),
					UserNote = x.UserNote
				}));

			return Ok(responseModel);
		}

		//TODO Chat
	}
}
