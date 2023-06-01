using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ODF.API.Registration.SettingModels;

namespace ODF.API.Controllers.Base
{
	[ApiController]
	[Route("api/[controller]")]
	public abstract class BaseController : Controller
	{
		private readonly IMediator _mediator;
		private readonly ApiSettings _settings;

		public IMediator Mediator => _mediator;
		public ApiSettings ApiSettings => _settings;

		protected BaseController(IMediator mediator, IOptions<ApiSettings> apiSettings)
		{
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
			_settings = apiSettings.Value ?? throw new ArgumentNullException(nameof(apiSettings));
		}
	}
}
