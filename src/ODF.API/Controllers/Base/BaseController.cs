using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using ODF.API.Extensions;
using ODF.API.Registration.SettingModels;
using ODF.API.ResponseModels.Common;
using ODF.API.ResponseModels.Common.Forms;
using ODF.AppLayer.Services.Interfaces;

namespace ODF.API.Controllers.Base
{
	[ApiController]
	[Route("/{countryCode}/[controller]")]
	public abstract class BaseController : Controller
	{
		private readonly IMediator _mediator;
		private readonly ApiSettings _settings;
		private readonly string _baseUrl;
		private readonly IActionDescriptorCollectionProvider _adcp;
		private readonly ITranslationsProvider _translationsProvider;

		public IMediator Mediator => _mediator;

		public string ApiBaseUrl => _baseUrl;

		public ITranslationsProvider TranslationsProvider => _translationsProvider;

		protected BaseController(IMediator mediator, IOptions<ApiSettings> apiSettings, IActionDescriptorCollectionProvider adcp, ITranslationsProvider translationsProvider)
		{
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
			_settings = apiSettings.Value ?? throw new ArgumentNullException(nameof(apiSettings));
			_adcp = adcp ?? throw new ArgumentNullException(nameof(adcp));
			_translationsProvider = translationsProvider ?? throw new ArgumentNullException(nameof(translationsProvider));

			_baseUrl = _settings.ApiUrl;
		}

		internal NamedAction GetNamedAction(string actionIdentifier, string actionName, string rel, Form? actionForm = null, string? countryCode = null)
			=> _adcp.GetNamedAction(HttpContext, ApiBaseUrl, actionIdentifier, actionName, rel, actionForm, countryCode);

		internal AppAction GetAppAction(string actionIdentifier, string rel, Form? actionForm = null, string? countryCode = null)
			=> _adcp.GetAppAction(HttpContext, ApiBaseUrl, actionIdentifier, rel, actionForm, countryCode);

		internal AppAction GetQueriedAppAction(string actionIdentifier, string rel, Dictionary<string, string> queryParams, Form? actionForm = null)
		 => _adcp.GetQueriedAppAction(HttpContext, ApiBaseUrl, actionIdentifier, rel, queryParams, actionForm);

		internal string GetRelativeUrlByAction(string actionIdentifier)
			=> _adcp.GetRelativeUrlByAction(HttpContext, actionIdentifier);
	}
}
