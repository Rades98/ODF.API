using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using ODF.API.Attributes.HtttpMethodAttributes;
using ODF.API.Controllers.Base;
using ODF.AppLayer.CQRS.User.Queries;
using ODF.AppLayer.Services.Interfaces;
using ODF.Domain.Constants;
using ODF.Domain.SettingModels;

namespace ODF.API.Controllers
{
	public class DataSourceController : BaseController
	{
		public DataSourceController(IMediator mediator, IOptions<ApiSettings> apiSettings, IActionDescriptorCollectionProvider adcp, ITranslationsProvider translationsProvider) : base(mediator, apiSettings, adcp, translationsProvider)
		{
		}

		[HttpGet("user", Name = nameof(GetUsers))]
		[Authorize(Roles = UserRoles.Admin)]
		[ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
		[CountryCodeFilter("cz")]
		public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
		{
			var res = await Mediator.Send(new GetAllUserNamesQuery(), cancellationToken);

			return Ok(res);
		}
	}
}
